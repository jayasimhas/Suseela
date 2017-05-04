using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Log = Sitecore.Diagnostics.Log;
using Informa.Library.Utilities.Extensions;
using System;
using System.Diagnostics;

namespace Informa.Web.Controllers
{
    [Route]
    public class PersonalizedEmailController : ApiController
    {
        private readonly EmailUtil _emailUtil;

        public PersonalizedEmailController(EmailUtil emailUtil)
        {

            _emailUtil = emailUtil;
        }

        [HttpPost]
        public HttpResponseMessage Post()
        {
            var response = new HttpResponseMessage();
            try
            {
                string data = Request.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(data))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                Stopwatch sw = Stopwatch.StartNew();
                var emailContent = _emailUtil.CreatePersonalizedEmailBody(data);
                StringExtensions.WriteSitecoreLogs("Time taken to Generate Personalized Email Body", sw, "CreatePersonalizedEmailBody");
                response.Content = new StringContent(emailContent, Encoding.UTF8, "text/html");
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                Log.Error("Could Not Generate Personalized Email Content", e, this);
                return response;
            }
        }

    }
}
