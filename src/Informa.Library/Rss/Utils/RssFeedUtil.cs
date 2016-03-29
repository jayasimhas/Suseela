using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Velir.Search.Models;

namespace Informa.Library.Rss.Utils
{
    public class RssFeedUtil
    {
        private readonly XNamespace atom = "http://www.w3.org/2005/Atom";
        protected string _hostName;
        protected ItemReferences _itemReferences;
        protected ISitecoreContext _sitecoreContext;

        public RssFeedUtil(ISitecoreContext sitecoreContext, string hostName, ItemReferences itemReferences)
        {
            _sitecoreContext = sitecoreContext;
            _hostName = hostName;
            _itemReferences = itemReferences;
        }

        public SyndicationFeed GetSearchRssFeed(ISearch_Rss_Feed rssFeedItem)
        {
            var feed = new SyndicationFeed();
            feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName),
                atom.NamespaceName);

            feed.Title = new TextSyndicationContent(GetSearchFeedTitle());
            feed.Language = "en-US";

            feed = AddFeedLinksToFeed(feed);
            feed = AddCopyrightTextToFeed(feed, _sitecoreContext, _itemReferences.SiteConfig);

            return feed;
        }

        public string GetSearchApiUrl(ISearch_Rss_Feed rssFeedItem)
        {
            if (rssFeedItem == null)
            {
                Log.Error("Error Getting RSS Feed Item.",this);
                return string.Empty;
            }

            Item searchPageItem = Sitecore.Context.Database.GetItem(rssFeedItem.Search_Page.ToString());

            if (searchPageItem == null)
            {
                Log.Error("Error Search Page Item For RSS Feed.", this);
                return string.Empty;
            }

            ISearch_Listing searchListing = searchPageItem.GlassCast<ISearch_Listing>(inferType: true);

            //Build the base api URL
            var searchPageId = searchPageItem.ID.ToGuid().ToString("D").ToLower();
            var feedUrl = string.Format("{0}://{1}{2}?pId={3}", HttpContext.Current.Request.Url.Scheme, _hostName,
                searchListing.Base_Endpoint_Url, searchPageId);

            //If the "Include Url Parameters" is checked then any url params are passed along
            //to the api, otherwise they will be stripped
            var useDefaultParams = false;
            if (rssFeedItem.Include_Url_Parameters)
            {
                if (Context.RawUrl.Contains("?"))
                {
                    var urlParts = Context.RawUrl.Split('?');

                    if (urlParts.Length == 2)
                    {
                        feedUrl = string.Format("{0}&{1}", feedUrl, urlParts[1]);
                    }
                    else
                    {
                        useDefaultParams = true;
                    }
                }
            }
            else
            {
                useDefaultParams = true;
            }

            //If we are using the default parameters then build the url string for the api
            //from the Sitecore Sort item.
            if (useDefaultParams)
            {
                var defaultSortBy = "";
                var defaultSortOrder = "";

                //If the default sort has not been chosen fall back to relevance ascending sorting
                if (searchListing.Default_Sort_Order == null)
                {
                    defaultSortBy = "relevance";
                    defaultSortOrder = "asc";
                }
                else
                {
                    defaultSortOrder = "asc";
                    if (!searchListing.Default_Sort_Order.Sort_Ascending)
                    {
                        defaultSortOrder = "desc";
                    }

                    defaultSortBy = searchListing.Default_Sort_Order.Key;
                }

                feedUrl = string.Format("{0}&sortBy={1}&sortOrder={2}", feedUrl, defaultSortBy, defaultSortOrder);
            }

            return feedUrl;
        }

        /// <summary>
        ///     Retrieves Sitecore items from the search API
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        private IEnumerable<Item> GetItemsFromApi(string apiUrl)
        {
            var client = new WebClient();
            var content = client.DownloadString(apiUrl);

            var results = JsonConvert.DeserializeObject<SearchResults>(content);

            return
                results.results.Select(searchResult => Context.Database.GetItem(searchResult.ItemId))
                    .Where(theItem => theItem != null)
                    .ToList();
        }

        /// <summary>
        ///     Build the url and get the sitecore items dicated by the content in the Search Rss Feed item
        /// </summary>
        /// <param name="rssFeedItem"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetSearchItems(ISearch_Rss_Feed rssFeedItem)
        {
            var feedUrl = GetSearchApiUrl(rssFeedItem);
            return GetItemsFromApi(feedUrl);
        }

        /// <summary>
        ///     The title for the feed will add any search terms provided in the url
        /// </summary>
        /// <returns></returns>
        public string GetSearchFeedTitle()
        {
            var title = "Search Feed";

            if (Context.Request.QueryString["q"] != null)
            {
                title += ": " + Context.Request.QueryString["q"];
            }

            return title;
        }

        /// <summary>
        ///     Add the copyright text to the rss channel, this is pulled from Sitecore
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public SyndicationFeed AddCopyrightTextToFeed(SyndicationFeed feed, ISitecoreContext sitecoreContext,
            Guid siteConfigItemId)
        {
            var siteConfig = sitecoreContext.GetItem<ISite_Config>(siteConfigItemId);
            feed.Copyright = new TextSyndicationContent(siteConfig.Copyright_Text);

            return feed;
        }

        /// <summary>
        ///     Add the channel links
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public SyndicationFeed AddFeedLinksToFeed(SyndicationFeed feed)
        {
            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetSearchUrl(_hostName))));
            feed.ElementExtensions.Add(new XElement(atom + "link",
                new XAttribute("href", new Uri(GetSearchUrl(_hostName))), new XAttribute("rel", "self"),
                new XAttribute("type", "application/rss+xml")));

            return feed;
        }

        /// <summary>
        ///     Gets the search url for the search page, this is different than the search rss url
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public string GetSearchUrl(string hostName)
        {
            var searchUrlBase = string.Format("{0}://{1}/search#?", HttpContext.Current.Request.Url.Scheme, hostName);

            if (!Context.RawUrl.Contains("?"))
            {
                return searchUrlBase;
            }

            var urlParts = Context.RawUrl.Split('?');

            if (urlParts.Length != 2)
            {
                return searchUrlBase;
            }

            return HttpUtility.HtmlEncode(string.Format("{0}{1}", searchUrlBase, urlParts[1]));
        }
    }
}