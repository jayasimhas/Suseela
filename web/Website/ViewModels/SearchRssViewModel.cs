using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Utils;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Rss;
using Sitecore.Data.Items;
using Sitecore.Web;

namespace Informa.Web.ViewModels
{
    public class SearchRssViewModel
    {
        private readonly RssFeedUtil _rssFeedutil;
        private readonly RssItemUtil _rssItemUtil;

        public SearchRssViewModel()
        {
            ISitecoreContext sitecoreContext = new SitecoreContext();

            _rssItemUtil = new RssItemUtil(sitecoreContext);
            _rssFeedutil = new RssFeedUtil(sitecoreContext, WebUtil.GetHostName(), new ItemReferences());
        }

        /// <summary>
        ///     Returns the xml rss string
        /// </summary>
        /// <returns></returns>
        public string GetSearchRssXml()
        {
            Item currentItem = Sitecore.Context.Item;
            ISearch_Rss_Feed rssFeedItem = currentItem.GlassCast<ISearch_Rss_Feed>(inferType: true);

            var feed = _rssFeedutil.GetSearchRssFeed(rssFeedItem);

            var formatter = new Rss20FeedFormatter(feed);
            formatter.SerializeExtensionsAsAtom = false;

            var searchItems = _rssFeedutil.GetSearchItems(rssFeedItem);

            var items =
                searchItems.Select(searchItem => _rssItemUtil.GetSyndicationItemFromSitecore(searchItem)).ToList();

            feed.Items = items;

            var output = new StringBuilder();

            using (var writer = XmlWriter.Create(output, new XmlWriterSettings {Indent = true}))
            {
                feed.SaveAsRss20(writer);
                writer.Flush();
                return output.ToString();
            }
        }
    }
}