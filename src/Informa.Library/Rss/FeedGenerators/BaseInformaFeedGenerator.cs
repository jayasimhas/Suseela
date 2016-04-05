using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Library.Rss.Utils;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Sitecore.Web;

namespace Informa.Library.Rss.FeedGenerators
{
    public class BaseInformaFeedGenerator : IRssFeedGeneration
    {

        public SyndicationFeed GetRssFeed(I_Base_Rss_Feed rssFeed,ISitecoreContext sitecoreContext,IItemReferences itemReferences)
        {
            var feed = new SyndicationFeed();
            feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName),
                RssConstants.AtomNamespace.ToString());

            feed.Title = new TextSyndicationContent(rssFeed.Title);
            feed.Language = rssFeed.Language;
            feed.Description = new TextSyndicationContent(rssFeed.Description);

            feed = AddCopyrightTextToFeed(feed, sitecoreContext, Constants.ScripRootNode, rssFeed);
            feed = AddFeedLinksToFeed(feed, rssFeed);
            feed = AddWebMasterToFeed(feed, rssFeed);

            return feed;
        }

        private SyndicationFeed AddWebMasterToFeed(SyndicationFeed feed, I_Base_Rss_Feed rssFeed)
        {
            feed.ElementExtensions.Add(new XElement("webMaster",rssFeed.Web_Master));
            return feed;
        }

        public SyndicationFeed AddFeedLinksToFeed(SyndicationFeed feed, I_Base_Rss_Feed rssFeed)
        {
            if (!string.IsNullOrEmpty(rssFeed.Link))
            {
                feed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(rssFeed.Link)));

                feed.ElementExtensions.Add(new XElement(RssConstants.AtomNamespace + "link",
               new XAttribute("href", new Uri(rssFeed.Link)), new XAttribute("rel", "self"),
               new XAttribute("type", "application/rss+xml")));
            }

            return feed;
        }

        /// <summary>
        ///     Add the copyright text to the rss channel, this is pulled from Sitecore global config.  The value
        /// can be overridden per feed if this field is filled 
        /// </summary>
        /// <param name="feed"></param>
        /// <returns></returns>
        public SyndicationFeed AddCopyrightTextToFeed(SyndicationFeed feed, ISitecoreContext sitecoreContext,string siteConfigItemId, I_Base_Rss_Feed rssFeed)
        {
            if (string.IsNullOrEmpty(rssFeed.Copyright))
            {
                var siteConfig = sitecoreContext.GetItem<ISite_Root>(siteConfigItemId);
                feed.Copyright = new TextSyndicationContent(siteConfig.Copyright_Text);
            }
            else
            {
                feed.Copyright = new TextSyndicationContent(rssFeed.Copyright);
            }


            return feed;
        }

    }
}
