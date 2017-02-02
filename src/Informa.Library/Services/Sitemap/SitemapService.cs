using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Search;
using Informa.Library.Search.Results;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.ContentSearch.Linq;
using System;

namespace Informa.Library.Services.Sitemap
{
    public interface ISitemapService
    {
        string GetSitemapXML();
        string GetNewsSitemapXML();
    }

    [AutowireService(LifetimeScope.PerScope)]
    public class SitemapService : ISitemapService
    {
        protected readonly IProviderSearchContextFactory SearchContextFactory;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IArticleSearch ArticleSearcher;
        protected readonly ITextTranslator TextTranslator;

        protected readonly string Xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        protected readonly string DateFormat = "yyyy-MM-ddTHH:mm:ss%K";

        public SitemapService(
            IProviderSearchContextFactory searchContextFactory,
            ISitecoreContext context,
            IArticleSearch searcher,
            ITextTranslator translator)
        {
            SearchContextFactory = searchContextFactory;
            SitecoreContext = context;
            ArticleSearcher = searcher;
            TextTranslator = translator;
        }

        public string GetSitemapXML_Old()
        {
            var home = SitecoreContext.GetHomeItem<IHome_Page>();
            string domain = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}";

            IEnumerable<I___BasePage> items = GetAllPages(home._Path);

            //start xml doc
            XmlDocument doc = new XmlDocument();
            //xml declaration
            XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declarationNode);
            //urlset
            XmlNode urlsetNode = doc.CreateElement("urlset");
            //xmlnls
            XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns");
            xmlnsAttr.Value = Xmlns;
            urlsetNode.Attributes.Append(xmlnsAttr);
            doc.AppendChild(urlsetNode);

            //append an xml node for each item
            foreach (I___BasePage itm in items)
            {
                if (itm == null)
                    continue;

                //create location
                string url = string.Empty;
                try
                {
                    url = itm.Canonical_Link?.Url;
                }
                catch (Exception ex)
                {

                }

                if (string.IsNullOrEmpty(url))
                {
                    try
                    {
                        url = itm._Url;
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (string.IsNullOrEmpty(url))
                    continue;

                string pageUrl = url;
                if (pageUrl.StartsWith("/"))
                    pageUrl = $"{domain}{pageUrl}";

                //set pointer
                XmlNode lastNode = doc.LastChild;

                //create new node
                XmlNode urlNode = MakeNode(doc, "url");
                lastNode.AppendChild(urlNode);
                urlNode.AppendChild(MakeNode(doc, "loc", pageUrl));
            }

            return doc.OuterXml;
        }

        public string GetNewsSitemapXML()
        {
            var home = SitecoreContext.GetHomeItem<IHome_Page>();
            string publisherName = TextTranslator.Translate("Article.PubName");
            string domain = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}";

            IEnumerable<IArticle> items = GetNewsPages(home._Path);

            //start xml doc
            XmlDocument doc = new XmlDocument();
            //xml declaration
            XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declarationNode);
            //urlset
            XmlNode urlsetNode = doc.CreateElement("urlset");
            //xmlnls
            XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns");
            xmlnsAttr.Value = Xmlns;
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
                urlNode.AppendChild(MakeNode(doc, "loc", $"{domain}/{itm.Article_Number}/{itm._Name}"));

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
                newsNode.AppendChild(MakeNode(doc, "news:publication_date", itm.Actual_Publish_Date.ToString(DateFormat)));
                var encodedItemTitle = HttpUtility.HtmlEncode(itm.Title);
                newsNode.AppendChild(MakeNode(doc, "news:title", HttpUtility.HtmlDecode(encodedItemTitle)));
                newsNode.AppendChild(MakeNode(doc, "news:keywords", (itm.Taxonomies != null && itm.Taxonomies.Any()) ? string.Join(",", itm.Taxonomies.Select(a => a.Item_Name)) : string.Empty));
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

        private IEnumerable<I___BasePage> GetAllPages(string startPath)
        {
            int pageNo = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);

            using (var context = SearchContextFactory.Create())
            {
                var query = context.GetQueryable<GeneralContentResult>()
                    .Filter(j
                    => (j.TemplateId == IGeneral_Content_PageConstants.TemplateId || j.TemplateId == IArticleConstants.TemplateId || j.TemplateId == ITopic_PageConstants.TemplateId)
                    && j.Path.StartsWith(startPath.ToLower())
                    && !j.ExcludeFromGoogleSearch);

                var results = query.GetResults();

                var pages = results.Hits.Select(h => SitecoreContext.GetItem<I___BasePage>(h.Document.ItemId.Guid)).Where(a => a != null);
                return (!pages.Any())
                ? Enumerable.Empty<I___BasePage>()
                : pages;
            }
        }

        private IEnumerable<IArticle> GetNewsPages(string startPath)
        {
            using (var context = SearchContextFactory.Create())
            {
                var query = context.GetQueryable<ArticleSearchResultItem>()
                    .Filter(i => i.TemplateId == IArticleConstants.TemplateId)
                    .Where(j => j.Path.StartsWith(startPath.ToLower()) && j.ActualPublishDate > DateTime.Now.AddDays(-90));

                query = query.OrderByDescending(i => i.ActualPublishDate);
                var results = query.GetResults();

                var articles = results.Hits.Select(h => SitecoreContext.GetItem<IArticle>(h.Document.ItemId.Guid)).Where(a => a != null);

                return (!articles.Any())
                ? Enumerable.Empty<IArticle>()
                : articles;
            }
        }

        public string GetSitemapXML()
        {
            string xmlResult = string.Empty;
            ///////////////////////////////////////////////////
            string pageNo = HttpContext.Current.Request.QueryString["page"];
            

            int page = 0;
            int.TryParse(pageNo, out page);

            if (page == 0)
            {

                #region Get Sitemap Index
                var home = SitecoreContext.GetHomeItem<IHome_Page>();
                string domain = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}";
                int pageSize = 200; // To Be Configured Later
                int TotalArticles = GetAllPages(home._Path).Count();
                int TotalCount = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalArticles) / Convert.ToDecimal(pageSize)));

                //start xml doc
                XmlDocument doc = new XmlDocument();
                //xml declaration
                XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(declarationNode);
                //urlset
                XmlNode urlsetNode = doc.CreateElement("sitemapindex");
                //xmlnls
                XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns");
                xmlnsAttr.Value = Xmlns;
                urlsetNode.Attributes.Append(xmlnsAttr);
                doc.AppendChild(urlsetNode);

                //append an xml node for each item
                for (int i = 1; i <= TotalCount; i++)
                {

                    //create location
                    string url = string.Empty;
                    try
                    {
                        url = "http://" + HttpContext.Current.Request.Url.Host + "/sitemap.xml?page=" + i;
                    }
                    catch (Exception ex)
                    {

                    }
                    //set pointer
                    XmlNode lastNode = doc.LastChild;

                    //create new node
                    XmlNode sitemapNode = MakeNode(doc, "sitemap");
                    lastNode.AppendChild(sitemapNode);
                    sitemapNode.AppendChild(MakeNode(doc, "loc", url));
                }

                xmlResult = doc.OuterXml;
                #endregion
            }
            else
            {
                #region Get Sitemap Page
                var home = SitecoreContext.GetHomeItem<IHome_Page>();
                string domain = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}";
                //start xml doc
                XmlDocument doc = new XmlDocument();
                //xml declaration
                XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(declarationNode);
                IEnumerable<I___BasePage> items = GetAllPages(home._Path).Skip(200 * (page - 1)).Take(200);
                //urlset
                XmlNode urlsetNode = doc.CreateElement("urlset");
                //xmlnls
                XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns");
                xmlnsAttr.Value = Xmlns;
                urlsetNode.Attributes.Append(xmlnsAttr);
                doc.AppendChild(urlsetNode);

                //append an xml node for each item
                foreach (I___BasePage itm in items)
                {
                    if (itm == null)
                        continue;

                    //create location
                    string url = string.Empty;
                    try
                    {
                        url = itm.Canonical_Link?.Url;
                    }
                    catch (Exception ex)
                    {

                    }

                    if (string.IsNullOrEmpty(url))
                    {
                        try
                        {
                            url = itm._Url;
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (string.IsNullOrEmpty(url))
                        continue;

                    string pageUrl = url;
                    if (pageUrl.StartsWith("/"))
                        pageUrl = $"{domain}{pageUrl}";

                    //set pointer
                    XmlNode lastNode = doc.LastChild;

                    //create new node
                    XmlNode urlNode = MakeNode(doc, "url");
                    lastNode.AppendChild(urlNode);
                    urlNode.AppendChild(MakeNode(doc, "loc", pageUrl));
                }
                xmlResult = doc.OuterXml;
                #endregion
            }
            ///////////////////////////////////////////////////
            return xmlResult;
        }
    }
}
