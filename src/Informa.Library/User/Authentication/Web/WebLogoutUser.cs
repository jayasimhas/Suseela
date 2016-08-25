using Jabberwocky.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService]
    public class WebLogoutUser : IWebLogoutUser
    {
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IWebLogoutUserActions Actions;

        public WebLogoutUser(
            IAuthenticatedUserContext authenticatedUserContext,
            IWebLogoutUserActions actions)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            Actions = actions;
        }

        public void Logout()
        {
            var user = AuthenticatedUserContext.User;

            if (user != null)
                Actions.Process(user);

            AuthenticationManager.Logout();
        }
    }
}
