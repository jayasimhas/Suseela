using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;
using Velir.Search.Models;

namespace Informa.Library.Rss.ItemRetrieval
{
    public class RssSearchResultsItemRetrieval : IRssSitecoreItemRetrieval
    {
        public IEnumerable<Item> GetSitecoreItems(Item feedItem)
        {
            ISearch_Rss_Feed searchRssFeed = feedItem.GlassCast<ISearch_Rss_Feed>(inferType: true);

            var feedUrl = GetSearchApiUrl(searchRssFeed);
            return GetItemsFromApi(feedUrl);
        }

        public string GetSearchApiUrl(ISearch_Rss_Feed rssFeedItem)
        {
            if (rssFeedItem == null)
            {
                Log.Error("Error Getting RSS Feed Item.", this);
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
            var feedUrl = string.Format("{0}://{1}{2}?pId={3}", HttpContext.Current.Request.Url.Scheme, WebUtil.GetHostName(),
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
            try
            {
                var client = new WebClient();
                var content = client.DownloadString(apiUrl);

                var results = JsonConvert.DeserializeObject<SearchResults>(content);

                return
                    results.results.Select(searchResult => Context.Database.GetItem(searchResult.ItemId))
                        .Where(theItem => theItem != null)
                        .ToList();
            }
            catch (Exception exc)
            {
                Log.Error("Could not retrieve items from the search service for URL: " + apiUrl + " : " + exc.Message, this);
                return new List<Item>();
            }
        }
    }
}
