using System;
using System.Net;
using System.Web;
using System.Web.Http;
using Informa.Library.Globalization;
using Jabberwocky.Core.Caching;

namespace Informa.Web.Controllers
{
    [System.Web.Mvc.Route]
    public class RobotsTextController : ApiController
    {
        protected readonly ICacheProvider CacheProvider;
        protected readonly ITextTranslator TextTranslator;

        public RobotsTextController(
            ICacheProvider cacheProvider,
            ITextTranslator translator)
        {
            CacheProvider = cacheProvider;
            TextTranslator = translator;
        }

        [HttpGet]
        public void RobotsText()
        {
            string cacheKey = $"{HttpContext.Current.Request.Url.Host}.RobotsText";
            string text = CacheProvider.GetFromCache(cacheKey, BuildRobotsText);

            //write
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }

        private string BuildRobotsText() { 

            string path = HttpContext.Current.Request.Path.Replace("/", "").Replace(".txt", "");
            string url = $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}/{path}";

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                return client.DownloadString(url);
            }
        }
    }
}