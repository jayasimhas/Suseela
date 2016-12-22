using Informa.Library.User.Authentication.Web;
using Jabberwocky.Autofac.Attributes;
using System.Web.Mvc;

namespace Informa.Web.Areas.UserRequest
{
    [AutowireService]
    public class ProcessUserRequestController : Controller
    {
        protected readonly IWebAuthenticateUser AuthenticateWebUser;

        public ProcessUserRequestController(IWebAuthenticateUser authenticateWebUser)
        {
            AuthenticateWebUser = authenticateWebUser;
        }

        // GET: ProcessUserRequest
        public ActionResult Index(string code, string state)
        {
            var result = AuthenticateWebUser.Authenticate(code, GetCallbackUrl("/User/ProcessUserRequest"));
            return Redirect(state);
        }

        private string GetCallbackUrl(string url)
        {
            return $"{Request.RequestContext.HttpContext.Request.Url.Scheme}://{Request.RequestContext.HttpContext.Request.Url.Authority}{Request.RequestContext.HttpContext.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
    }
}