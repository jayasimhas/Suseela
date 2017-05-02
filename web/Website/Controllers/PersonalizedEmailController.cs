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
            var response = new HttpResponseMessage();
            try
            {
                string data = Request.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(data))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var emailContent = _emailUtil.CreatePersonalizedEmailBody(data);
                response.Content = new StringContent(emailContent, Encoding.UTF8, "text/html");
                return response;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }

    }
}
