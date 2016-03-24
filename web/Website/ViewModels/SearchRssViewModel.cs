using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Rss.Utils;
using Informa.Library.Utilities.References;
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
        /// Returns the xml rss string
        /// </summary>
        /// <returns></returns>
        public string GetSearchRssXml()
        {

            var feed = _rssFeedutil.GetSearchRssFeed();

            var formatter = new Rss20FeedFormatter(feed);
            formatter.SerializeExtensionsAsAtom = false;
           
         
            //XNamespace atom = "http://www.w3.org/2005/Atom";

            //feed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName), atom.NamespaceName);


            var searchItems = _rssFeedutil.GetSearchItems();

            var items = searchItems.Select(searchItem => _rssItemUtil.GetSyndicationItemFromSitecore(searchItem)).ToList();

            feed.Items = items;

            var output = new StringBuilder();

            using (var writer = XmlWriter.Create(output, new XmlWriterSettings {Indent = true}))
            {
                feed.SaveAsRss20(writer);
              //  formatter.WriteTo(writer);
                writer.Flush();

                return output.ToString();
                //rssString = rssString.Replace(":a10", ":atom");
                //return rssString.Replace("a10:", "atom:");
            }
        }
    }
}