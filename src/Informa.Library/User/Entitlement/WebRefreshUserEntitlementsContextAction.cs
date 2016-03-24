using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class WebRefreshUserEntitlementsContextAction : IWebLogoutUserAction, IWebLoginUserAction
    {
        protected readonly ISitecoreUserContext SitecoreUserContext;

        public WebRefreshUserEntitlementsContextAction(
            ISitecoreUserContext sitecoreUserContext)
        {
            SitecoreUserContext = sitecoreUserContext;
        }

        public void Process(IUser user)
        {
            SitecoreUserContext.RefreshEntitlements();
        }
    }
}