using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace Informa.Web.Areas.Admin.Attributes
{
    public class AdminOnlyAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            filterContext.Principal = null;
            filterContext.HttpContext.User = null;

            filterContext.Principal = AuthenticationManager.GetActiveUser();
            filterContext.HttpContext.User = filterContext.Principal;
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User as User;
            if (user == null || !user.Identity.IsAuthenticated || !user.IsAdministrator)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}