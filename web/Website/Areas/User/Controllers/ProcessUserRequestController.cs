using Informa.Library.SalesforceConfiguration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Site;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Autofac.Attributes;
using System.Web.Mvc;

namespace Informa.Web.Areas.UserRequest
{
    [AutowireService]
    public class ProcessUserRequestController : Controller
    {
        protected readonly IWebAuthenticateUser AuthenticateWebUser;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        private string _authorizationRequestEndPoint = "{0}/services/oauth2/authorize?response_type=code&client_id={1}&redirect_uri={2}&state={3}";

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
            return Redirect(state);
        }

        public ActionResult Register()
        {
            string authorizationRequestUrl = string.Format(_authorizationRequestEndPoint,
            SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Service_Url?.Url,
            SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Username,
            GetCallbackUrl("/User/ProcessUserRequest"), GetCallbackUrl(!string.IsNullOrEmpty(_siteRootItem?.Enrolment_Link.Url) ? _siteRootItem?.Enrolment_Link.Url : string.Empty));
            return Redirect(authorizationRequestUrl);
        }

        private string GetCallbackUrl(string url)
        {
            return $"{Request.RequestContext.HttpContext.Request.Url.Scheme}://{Request.RequestContext.HttpContext.Request.Url.Authority}{Request.RequestContext.HttpContext.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
    }
}