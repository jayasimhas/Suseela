using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Interfaces;
using Informa.Library.Rss.Utils;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Sitecore.Data.Items;
using Log = Sitecore.Diagnostics.Log;

namespace Informa.Web.ViewModels
{
    public class SearchRssViewModel
    {
        private ISitecoreContext _sitecoreContext;
        private ItemReferences _itemReferences;

        public SearchRssViewModel()
        {
            _sitecoreContext = new SitecoreContext();
            _itemReferences = new ItemReferences();
        }

        /// <summary>
        ///     Returns the xml rss string
        /// </summary>
        /// <returns></returns>
        public string GetSearchRssXml()
        {
            Item currentItem = Sitecore.Context.Item;
            I_Base_Rss_Feed rssFeedItem = currentItem.GlassCast<I_Base_Rss_Feed>(inferType: false);

            SyndicationFeed feed = null;

            var feedGenerator = GetFeedGenerator(rssFeedItem);
            if (feedGenerator == null)
            {
                Log.Error("Could Not Create RSS Feed Geneartor " + rssFeedItem.Rss_Feed_Generation, this);
                return string.Empty;
            }

            feed = feedGenerator.GetRssFeed(rssFeedItem, _sitecoreContext, _itemReferences);

            if (feed == null)
            {
                Log.Error("Could Not Create RSS Feed With " + rssFeedItem.Rss_Feed_Generation, this);
                return string.Empty;
            }

            var formatter = new Rss20FeedFormatter(feed);
            formatter.SerializeExtensionsAsAtom = false;

            var itemRetriever = GetItemRetriever(rssFeedItem);
            if (itemRetriever == null)
            {
                Log.Error("Could Not Create Item Retriever With " + rssFeedItem.Sitecore_Item_Retrieval, this);
                return string.Empty;
            }

            var rssItemGenerator = GetItemGenerator(rssFeedItem);
            if (rssItemGenerator == null)
            {
                Log.Error("Could Not Create Item Generator With " + rssFeedItem.Rss_Item_Generation, this);
                return string.Empty;
            }

            var sitecoreItems = itemRetriever.GetSitecoreItems(currentItem);

            List<SyndicationItem> syndicationItems = new List<SyndicationItem>();
            foreach (Item sitecoreItem in sitecoreItems)
            {
                var syndicationItem = rssItemGenerator.GetSyndicationItemFromSitecore(_sitecoreContext, sitecoreItem);
                if (syndicationItem == null)
                {
                    continue;
                    
                }

                syndicationItems.Add(syndicationItem);
            }

            feed.Items = syndicationItems;


            var output = new StringBuilder();

            using (var writer = XmlWriter.Create(output, new XmlWriterSettings {Indent = true}))
            {
                feed.SaveAsRss20(writer);
                writer.Flush();
                writer.Close();
                string rs = output.ToString().Replace("utf-16", "utf-8");
                rs = RemoveInvalidXmlChars(rs);
                return rs;
            }
        }

        private string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        public IRssFeedGeneration GetFeedGenerator(I_Base_Rss_Feed rssFeedItem)
        {
            Type feedGenerationType = Type.GetType(rssFeedItem.Rss_Feed_Generation);
            if (feedGenerationType != null)
            {
                return Activator.CreateInstance(feedGenerationType) as IRssFeedGeneration;
            }

            return null;
        }
        public IRssItemGeneration GetItemGenerator(I_Base_Rss_Feed rssFeedItem)
        {
            Type itemGenerationType = Type.GetType(rssFeedItem.Rss_Item_Generation);
            if (itemGenerationType != null)
            {
                return Activator.CreateInstance(itemGenerationType) as IRssItemGeneration;
            }

            return null;
        }
        public IRssSitecoreItemRetrieval GetItemRetriever(I_Base_Rss_Feed rssFeedItem)
        {
            Type itemRetrievalType = Type.GetType(rssFeedItem.Sitecore_Item_Retrieval);
            if (itemRetrievalType != null)
            {
                return Activator.CreateInstance(itemRetrievalType) as IRssSitecoreItemRetrieval;
            }

            return null;
        }
    }
}