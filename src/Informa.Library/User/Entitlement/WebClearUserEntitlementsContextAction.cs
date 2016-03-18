using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class WebClearUserEntitlementsContextAction : IWebLogoutUserAction, IWebLoginUserAction
    {
        protected readonly ISitecoreUserContext SitecoreUserContext;

        public WebClearUserEntitlementsContextAction(
            ISitecoreUserContext sitecoreUserContext)
        {
            SitecoreUserContext = sitecoreUserContext;
        }

        public void Process(IAuthenticatedUser user)
        {
            SitecoreUserContext.RefreshEntitlements();
        }
    }
}