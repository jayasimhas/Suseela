using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Net;
using System.Text;

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

        [HttpGet]
        public HttpResponseMessage Get(string userId)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = string.IsNullOrWhiteSpace(userId) ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var emailContent = _emailUtil.CreatePersonalizedEmailBody(userId);
                response.Content = new StringContent(emailContent, Encoding.UTF8, "text/html");
            }
            return response;
        }
    }
}
