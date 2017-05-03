using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Search.Utilities
{
    public class CookieAwareWebClients : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }

        public CookieAwareWebClients()
            : this(new CookieContainer())
        {
        }

        public CookieAwareWebClients(CookieContainer cookies)
        {
            this.CookieContainer = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        //protected override WebResponse GetWebResponse(WebRequest request)
        //{
        //    WebResponse response = base.GetWebResponse(request);
        //    String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

        //    if (setCookieHeader != null)
        //    {
        //        //do something if needed to parse out the cookie.
        //        if (setCookieHeader != null)
        //        {
        //            Cookie cookie = new Cookie(); //create cookie
        //            cookie.Domain=System.Configuration.ConfigurationManager.AppSettings["SSOSubdomain"];
        //            this.CookieContainer.Add(cookie);
        //        }
        //    }
        //    return response;
        //}
    }
}
