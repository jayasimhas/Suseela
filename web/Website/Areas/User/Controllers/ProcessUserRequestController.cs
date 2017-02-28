using Informa.Library.SalesforceConfiguration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Site;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Autofac.Attributes;
using System.Web.Mvc;
using Informa.Library.User.Authentication;

namespace Informa.Web.Areas.UserRequest
{
    [AutowireService]
    public class ProcessUserRequestController : Controller
    {
        protected readonly IWebAuthenticateUser AuthenticateWebUser;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public ProcessUserRequestController(IWebAuthenticateUser authenticateWebUser,
        ISalesforceConfigurationContext salesforceConfigurationContext, ISiteRootContext siteRootContext)
        {
            AuthenticateWebUser = authenticateWebUser;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SiteRootContext = siteRootContext;
        }

        private ISite_Root _siteRootItem => SiteRootContext?.Item;
        // GET: ProcessUserRequest
        public ActionResult Index(string code, string state)
        {
            var result = AuthenticateWebUser.Authenticate(code, GetCallbackUrl("/User/ProcessUserRequest"));
            if (!result.Success && result.State.Equals(AuthenticateUserResultState.Failure))
                return Redirect(state + "?ErrorStatus=" +"true");
            else
                return Redirect(state + "?login=success");
        }

        public ActionResult Register()
        {
            string authorizationRequestUrl = SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), GetCallbackUrl(_siteRootItem?.Enrolment_Link != null && !string.IsNullOrEmpty(_siteRootItem?.Enrolment_Link.Url) ? _siteRootItem?.Enrolment_Link.Url : string.Empty));
            return Redirect(authorizationRequestUrl);
        }

        private string GetCallbackUrl(string url)
        {
            return $"{Request.RequestContext.HttpContext.Request.Url.Scheme}://{Request.RequestContext.HttpContext.Request.Url.Authority}{Request.RequestContext.HttpContext.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
    }
}