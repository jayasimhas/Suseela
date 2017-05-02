using System.Net.Http;
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

        [HttpPost]
        public HttpResponseMessage Post()
        {
            string data = Request.Content.ReadAsStringAsync().Result;
            var response = new HttpResponseMessage();
            var emailContent = _emailUtil.CreatePersonalizedEmailBody(data);
            response.StatusCode = string.IsNullOrWhiteSpace(data) ? HttpStatusCode.BadRequest :
               string.IsNullOrWhiteSpace(emailContent) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
            response.Content = new StringContent(emailContent, Encoding.UTF8, "text/html");
            return response;
        }

    }
}
