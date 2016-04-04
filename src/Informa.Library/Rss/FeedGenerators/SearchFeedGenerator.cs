using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Library.Rss.Utils;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Sitecore;
using Sitecore.Web;

namespace Informa.Library.Rss.FeedGenerators
{
    public class SearchFeedGenerator : BaseInformaFeedGenerator, IRssFeedGeneration
    {
        public SearchFeedGenerator()
        {
        }

        public SyndicationFeed GetRssFeed(I_Base_Rss_Feed rssFeed, ISitecoreContext sitecoreContext, IItemReferences itemReferences)
        {
            SyndicationFeed feed = base.GetRssFeed(rssFeed, sitecoreContext, itemReferences);
            feed = AddFeedLinksToFeed(feed, rssFeed);

            feed.Title = new TextSyndicationContent(GetSearchFeedTitle());

            return feed;
        }

        /// <summary>
        ///     Add the channel links
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public SyndicationFeed AddFeedLinksToFeed(SyndicationFeed feed, I_Base_Rss_Feed rssFeed)
        {
            feed.ElementExtensions.Add(new XElement(RssConstants.AtomNamespace + "link",
                new XAttribute("href", new Uri(GetSearchUrl(WebUtil.GetHostName()))), new XAttribute("rel", "self"),
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

    }
}
