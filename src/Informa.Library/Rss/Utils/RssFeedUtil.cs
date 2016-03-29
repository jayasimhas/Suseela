using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.Utils
{
    public class RssFeedUtil
    {
        XNamespace atom = "http://www.w3.org/2005/Atom";
        protected ISitecoreContext _sitecoreContext;
        protected string _hostName;
        protected ItemReferences _itemReferences;

        public RssFeedUtil(ISitecoreContext sitecoreContext, string hostName, ItemReferences itemReferences)
        {
            _sitecoreContext = sitecoreContext;
            _hostName = hostName;
            _itemReferences = itemReferences;
        }

        public SyndicationFeed GetSearchRssFeed()
        {
            SyndicationFeed feed = new SyndicationFeed();
            feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName), atom.NamespaceName);

            feed.Title = new TextSyndicationContent(GetSearchFeedTitle());
            feed.Language = "en-US";
           
            feed = AddFeedLinksToFeed(feed);
            feed = AddCopyrightTextToFeed(feed, _sitecoreContext, _itemReferences.SiteConfig);

            return feed;
        }

        public IEnumerable<Item> GetSearchItems()
        {
            string searchPageId = _itemReferences.SearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
            string url = string.Format("{0}://{1}/api/informasearch?pId={2}", HttpContext.Current.Request.Url.Scheme, _hostName, searchPageId);

            if (Context.RawUrl.Contains("?"))
            {
                var urlParts = Context.RawUrl.Split('?');

                if (urlParts.Length == 2)
                {
                    url = string.Format("{0}&{1}", url, urlParts[1]);
                }
            }
            else
            {
                url = string.Format("{0}&sortBy=relevance&sortOrder=asc", url);
            }

            var client = new WebClient();
            var content = client.DownloadString(url);

            var results = JsonConvert.DeserializeObject<SearchResults>(content);

            var resultItems = new List<Item>();

            foreach (var searchResult in results.results)
            {
                var theItem = Context.Database.GetItem(searchResult.ItemId);

                if (theItem == null)
                {
                    continue;
                }

                resultItems.Add(theItem);
            }

            return resultItems;
        }

        public string GetSearchFeedTitle()
        {
            var title = "Search Feed";

            if (Sitecore.Context.Request.QueryString["q"] != null)
            {
                title += ": " + Sitecore.Context.Request.QueryString["q"];
            }

            return title;
        }

        /// <summary>
        /// Add the copyright text to the rss channel, this is pulled from Sitecore
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public SyndicationFeed AddCopyrightTextToFeed(SyndicationFeed feed,ISitecoreContext sitecoreContext,Guid siteConfigItemId)
        {
            var siteConfig = sitecoreContext.GetItem<ISite_Config>(siteConfigItemId);
            feed.Copyright = new TextSyndicationContent(siteConfig.Copyright_Text);

            return feed;
        }

        /// <summary>
        /// Add the channel links
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public SyndicationFeed AddFeedLinksToFeed(SyndicationFeed feed)
        {
            feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetSearchUrl(_hostName))));            
            feed.ElementExtensions.Add(new XElement(atom + "link", new XAttribute("href", new Uri(GetSearchUrl(_hostName))), new XAttribute("rel", "self"), new XAttribute("type", "application/rss+xml")));

            return feed;
        }


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
