using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml;
using Informa.Library.Globalization;
using Jabberwocky.Core.Caching;

namespace Informa.Web.Controllers
{
    [System.Web.Mvc.Route]
    public class SitemapController : ApiController
    {
        protected readonly ICacheProvider CacheProvider;
        protected readonly ITextTranslator TextTranslator;

        public SitemapController(
            ICacheProvider cacheProvider,
            ITextTranslator translator)
        {
            CacheProvider = cacheProvider;
            TextTranslator = translator;
        }

        [HttpGet]
        public void SitemapXml()
        {
            int pageNo = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
            string SitemapQueryString = string.Empty;
            if (pageNo != 0)
            {
                SitemapQueryString = "?page=" + pageNo;
            }
            string path = HttpContext.Current.Request.Path.Replace("/", "").Replace(".xml", "") + SitemapQueryString;
            string cacheKey = $"{HttpContext.Current.Request.Url.Host}.{path}.Sitemap";
            string xml = CacheProvider.GetFromCache(cacheKey, () => BuildSitemapXml(path));

            //provide default
            if (xml.Length == 0)
                xml = new XmlDocument().OuterXml;

            //write
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
            HttpContext.Current.Response.End();
        }

        private string BuildSitemapXml(string path)
        {
            string url = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}/{path}";
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                return client.DownloadString(url);
            }
        }
    }
}