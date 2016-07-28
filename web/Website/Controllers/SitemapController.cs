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
            IApiOutputCache cache,
            ITextTranslator translator)
        {
            Cache = cache;
            TextTranslator = translator;
        }

        [HttpGet]
        public void SitemapXml()
        {
            string path = HttpContext.Current.Request.Path.Replace("/", "").Replace(".xml", "");
            //string cacheKey = $"{HttpContext.Current.Request.Url.Host}.{path}.Sitemap";
            //string xml = string.Empty;
            //if (Cache.Contains(cacheKey))
            //{
            //    //check cache
            //    xml = Cache.Get<string>(cacheKey);
            //}
            //else
            //{
            //    //get it fresh
            string url = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}/{path}";
            //    using (WebClient client = new WebClient())
            //    {
            //        client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            //        xml = client.DownloadString(url);
            //        //cache it
            //        if (!string.IsNullOrEmpty(xml))
            //        {
            //            int minutes = 0;
            //            if (!int.TryParse(TextTranslator.Translate("Sitemap.CacheMin"), out minutes))
            //                minutes = 5;
            //            DateTimeOffset dto = new DateTimeOffset(DateTime.Now).ToOffset(TimeSpan.FromMinutes(minutes));
            //            Cache.Add(cacheKey, xml, dto);
            //        }
            //    }
            //}

            ////provide default
            //if (xml.Length == 0)
            //    xml = new XmlDocument().OuterXml;

            ////write
            //HttpContext.Current.Response.StatusCode = 200;
            //HttpContext.Current.Response.ContentType = "text/xml";
            //HttpContext.Current.Response.Write(xml);
            //HttpContext.Current.Response.End();
            HttpContext.Current.Response.Redirect(url);
        }
    }
}