using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml;
using Informa.Library.Globalization;
using WebApi.OutputCache.Core.Cache;

namespace Informa.Web.Controllers
{
    [System.Web.Mvc.Route]
    public class RobotsTextController : ApiController
    {
        protected readonly IApiOutputCache Cache;
        protected readonly ITextTranslator TextTranslator;

        public RobotsTextController(
            IApiOutputCache cache,
            ITextTranslator translator)
        {
            Cache = cache;
            TextTranslator = translator;
        }

        [HttpGet]
        public void RobotsText()
        {
            string cacheKey = $"{HttpContext.Current.Request.Url.Host}.RobotsText";
            string text = string.Empty;
            if (Cache.Contains(cacheKey))
            {
                //check cache
                text = Cache.Get<string>(cacheKey);
            }
            else
            {
                //get it fresh
                string path = HttpContext.Current.Request.Path.Replace("/", "").Replace(".txt", "");
                string url = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}/{path}";
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                    text = client.DownloadString(url);
                    //cache it
                    if (!string.IsNullOrEmpty(text))
                    {
                        int minutes = 0;
                        if (!int.TryParse(TextTranslator.Translate("Robots.CacheMin"), out minutes))
                            minutes = 5;
                        DateTimeOffset dto = new DateTimeOffset(DateTime.Now).ToOffset(TimeSpan.FromMinutes(minutes));
                        Cache.Add(cacheKey, text, dto);
                    }
                }
            }
            
            //write
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }
    }
}