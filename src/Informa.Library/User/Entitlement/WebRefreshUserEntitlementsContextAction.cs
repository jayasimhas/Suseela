using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class WebRefreshUserEntitlementsContextAction : IWebLogoutUserAction, IWebLoginUserAction
    {
        protected readonly IAuthenticatedUserEntitlementsContext UserEntitlementContext;

        public WebRefreshUserEntitlementsContextAction(
			IAuthenticatedUserEntitlementsContext userEntitlementContext)
        {
            UserEntitlementContext = userEntitlementContext;
        }

        public void Process(IUser user)
        {
            UserEntitlementContext.RefreshEntitlements();
        }
    }
}