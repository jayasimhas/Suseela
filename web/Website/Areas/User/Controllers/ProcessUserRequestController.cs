using Informa.Library.SalesforceConfiguration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Site;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Autofac.Attributes;
using System.Web.Mvc;
using Informa.Library.User.Authentication;
using System;
using System.Web;

namespace Informa.Web.Areas.UserRequest
{
    [AutowireService]
    public class ProcessUserRequestController : Controller
    {
        protected readonly IWebAuthenticateUser AuthenticateWebUser;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IWebLogoutUser LogoutWebUser;

        public ProcessUserRequestController(IWebAuthenticateUser authenticateWebUser,
        ISalesforceConfigurationContext salesforceConfigurationContext, ISiteRootContext siteRootContext, IWebLogoutUser logoutWebUser)
        {
            AuthenticateWebUser = authenticateWebUser;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SiteRootContext = siteRootContext;
            LogoutWebUser = logoutWebUser;
        }

        private ISite_Root _siteRootItem => SiteRootContext?.Item;
        // GET: ProcessUserRequest
        public ActionResult Index(string code, string state)
        {
            string vertcal = GetParam(state, "vid");
            var result = AuthenticateWebUser.Authenticate(code, GetCallbackUrl("/User/ProcessUserRequest"),vertcal);
            if (!result.Success && result.State.Equals(AuthenticateUserResultState.Failure))
                return Redirect(state + "?ErrorStatus=" +"true");
            else
                return Redirect(state);
        }

        public ActionResult Register()
        {
            string authorizationRequestUrl = SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), GetCallbackUrl(_siteRootItem?.Enrolment_Link != null && !string.IsNullOrEmpty(_siteRootItem?.Enrolment_Link.Url) ? _siteRootItem?.Enrolment_Link.Url+"?"+ _siteRootItem?.Enrolment_Link.Query : string.Empty));
            return Redirect(authorizationRequestUrl);
        }

        public ActionResult Logout()
        {
            LogoutWebUser.Logout();
            return Redirect(GetCallbackUrl("/"));
        }

        private string GetCallbackUrl(string url)
        {
            return $"{Request.RequestContext.HttpContext.Request.Url.Scheme}://{Request.RequestContext.HttpContext.Request.Url.Authority}{Request.RequestContext.HttpContext.Request.ApplicationPath.TrimEnd('/')}{url}";
        }

        private string GetParam(string url, string param)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var val = query.Get("vid");
            return val;
        }
    }
}