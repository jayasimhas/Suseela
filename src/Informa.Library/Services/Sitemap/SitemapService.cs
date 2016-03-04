using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Services.Sitemap
{
    public interface ISitemapService
    {
        string GetNewsSitemapXML();
    }

    [AutowireService(LifetimeScope.SingleInstance)]
    public class SitemapService : ISitemapService
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IArticleSearch ArticleSearcher;
        protected readonly ITextTranslator TextTranslator;

        public SitemapService(
            ISitecoreContext context,
            IArticleSearch searcher,
            ITextTranslator translator)
        {
            SitecoreContext = context;
            ArticleSearcher = searcher;
            TextTranslator = translator;
        }

        public string GetNewsSitemapXML()
        {
            var home = SitecoreContext.GetHomeItem<IHome_Page>();
            string publisherName = TextTranslator.Translate("Article.PubName");
            string domain = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}";

            IEnumerable<IArticle> items = GetPages(home._Path);

            //start xml doc
            XmlDocument doc = new XmlDocument();
            //xml declaration
            XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declarationNode);
            //urlset
            XmlNode urlsetNode = doc.CreateElement("urlset");
            //xmlnls
            XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns");
            xmlnsAttr.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
            urlsetNode.Attributes.Append(xmlnsAttr);
            //xmnls:news
            XmlAttribute xmlnsNewsAttr = doc.CreateAttribute("xmlns:news");
            xmlnsNewsAttr.Value = "http://www.google.com/schemas/sitemap-news/0.9";
            urlsetNode.Attributes.Append(xmlnsNewsAttr);

            doc.AppendChild(urlsetNode);

            //append an xml node for each item
            foreach (IArticle itm in items)
            {
                //set pointer
                XmlNode lastNode = doc.LastChild;

                //create new node
                XmlNode urlNode = MakeNode(doc, "url");
                lastNode.AppendChild(urlNode);

                //create location
                urlNode.AppendChild(MakeNode(doc, "loc", $"{domain}{itm._Url}"));

                //create news
                XmlNode newsNode = MakeNode(doc, "news:news");
                urlNode.AppendChild(newsNode);

                //create publication
                XmlNode pubNode = MakeNode(doc, "news:publication");
                newsNode.AppendChild(pubNode);

                //create name and langauge
                pubNode.AppendChild(MakeNode(doc, "news:name", publisherName));
                pubNode.AppendChild(MakeNode(doc, "news:language", itm._Language.CultureInfo.TwoLetterISOLanguageName));

                //create access, pub date, title and keywords
                newsNode.AppendChild(MakeNode(doc, "news:access", "Subscription"));
                newsNode.AppendChild(MakeNode(doc, "news:publication_date", itm.Actual_Publish_Date.ToString("yyyy-MM-ddTHH:mm:ss%K")));
                newsNode.AppendChild(MakeNode(doc, "news:title", itm.Title));
                newsNode.AppendChild(MakeNode(doc, "news:keywords", itm.Meta_Keywords));
            }

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"
                    xmlns:news="http://www.google.com/schemas/sitemap-news/0.9">
                <url>
                <loc>http://www.example.org/business/article55.html</loc>
                <news:news>
                    <news:publication>
                    <news:name>The Example Times</news:name>
                    <news:language>en</news:language>
                    </news:publication>
                    <news:access>Subscription</news:access>
                    <news:publication_date>2008-12-23</news:publication_date>
                    <news:title>Companies A, B in Merger Talks</news:title>
                    <news:keywords>business, merger, acquisition, A, B</news:keywords>
                </news:news>
                </url>
            </urlset>
            */

            return doc.OuterXml;
        }

        private XmlNode MakeNode(XmlDocument doc, string nodeName, string nodeValue = "")
        {
            XmlNode newNode = doc.CreateElement(nodeName);
            if (!string.IsNullOrEmpty(nodeValue))
                newNode.AppendChild(doc.CreateTextNode(nodeValue));
            return newNode;
        }

        private IEnumerable<IArticle> GetPages(string startPath)
        {
            //query for all items
            var articles = ArticleSearcher.NewsSitemapSearch(startPath);

            return (!articles.Any())
                ? Enumerable.Empty<IArticle>()
                : articles;
        }
    }
}
