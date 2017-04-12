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
using Informa.Library.Rss.FeedGenerators;
using Informa.Library.Rss.ItemGenerators;
using Informa.Library.Rss.ItemRetrieval;
using System.Diagnostics;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.ViewModels
{
    public class SearchRssViewModel
    {
        private ISitecoreContext _sitecoreContext;
        private ItemReferences _itemReferences;
        private IRssFeedGeneration _RssFeedGeneration;
        private IRssItemGeneration _RssItemGeneration;
        private IRssSitecoreItemRetrieval _RssSitecoreItemRetrieval;

        public SearchRssViewModel()
        {
            _sitecoreContext = new SitecoreContext();
            _itemReferences = new ItemReferences();
            _RssFeedGeneration = new SearchFeedGenerator();
            _RssItemGeneration = new SearchRssItemGenerator();
            _RssSitecoreItemRetrieval = new RssSearchResultsItemRetrieval();
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
            Stopwatch sw = Stopwatch.StartNew();
            var feedGenerator = _RssFeedGeneration;
            StringExtensions.WriteSitecoreLogs("Time taken to Create Feedgenerator", sw, "feedGenerator");
            if (feedGenerator == null)
            {
                Log.Error("Could Not Create RSS Feed Geneartor " + rssFeedItem.Rss_Feed_Generation, this);
                return string.Empty;
            }
            sw = Stopwatch.StartNew();
            feed = feedGenerator.GetRssFeed(rssFeedItem, _sitecoreContext, _itemReferences);
            StringExtensions.WriteSitecoreLogs("Time taken to Create GetRssFeed", sw, "GetRssFeed");

            if (feed == null)
            {
                Log.Error("Could Not Create RSS Feed With " + rssFeedItem.Rss_Feed_Generation, this);
                return string.Empty;
            }

            var formatter = new Rss20FeedFormatter(feed);
            formatter.SerializeExtensionsAsAtom = false;
            sw = Stopwatch.StartNew();
            var itemRetriever = _RssSitecoreItemRetrieval;
            StringExtensions.WriteSitecoreLogs("Time taken to Create itemRetriever", sw, "itemRetriever");
            if (itemRetriever == null)
            {
                Log.Error("Could Not Create Item Retriever With " + rssFeedItem.Sitecore_Item_Retrieval, this);
                return string.Empty;
            }

            var rssItemGenerator = _RssItemGeneration;
            if (rssItemGenerator == null)
            {
                Log.Error("Could Not Create Item Generator With " + rssFeedItem.Rss_Item_Generation, this);
                return string.Empty;
            }
            sw = Stopwatch.StartNew();
            var sitecoreItems = itemRetriever.GetSitecoreItems(currentItem);
            StringExtensions.WriteSitecoreLogs("Time taken to Create sitecoreItems", sw, "GetSitecoreItems");
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

            using (var writer = XmlWriter.Create(output, new XmlWriterSettings { Indent = true }))
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

    }
}