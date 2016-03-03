using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.ContentCuration.Search.Extensions;
using Informa.Library.Globalization;
using Informa.Library.Search;
using Informa.Library.Search.Extensions;
using Informa.Library.Services.Sitemap;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;
using WebApi.OutputCache.Core.Cache;

namespace Informa.Web.Controllers
{
    [System.Web.Mvc.Route]
    public class SitemapController : ApiController
    {
        protected readonly IApiOutputCache Cache;
        protected readonly ITextTranslator TextTranslator;

        public SitemapController(
            IApiOutputCache cache,
            ITextTranslator translator)
        {
            Cache = cache;
            TextTranslator = translator;
        }

        [HttpGet]
        public void NewsSitemapXml()
        {
            string cacheKey = $"{HttpContext.Current.Request.Url.Host}.Sitemap";
            string xml = string.Empty;
            if (Cache.Contains(cacheKey))
            {
                //check cache
                xml = Cache.Get<string>(cacheKey);
            }
            else
            {
                //get it fresh
                string path = HttpContext.Current.Request.Path.Replace("/", "").Replace(".xml", "");
                string url = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}/{path}";
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                    xml = client.DownloadString(url);
                    //cache it
                    if (!string.IsNullOrEmpty(xml))
                    {
                        int minutes = 0;
                        if (!int.TryParse(TextTranslator.Translate("Sitemap.CacheMin"), out minutes))
                            minutes = 5;
                        DateTimeOffset dto = new DateTimeOffset(DateTime.Now).ToOffset(TimeSpan.FromMinutes(minutes));
                        Cache.Add(cacheKey, xml, dto);
                    }
                }
            }
            
            //provide default
            if (xml.Length == 0)
                xml = new XmlDocument().OuterXml;

            //write
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.ContentType = "text/xml";
            HttpContext.Current.Response.Write(xml);
            HttpContext.Current.Response.End();
        }
    }
}