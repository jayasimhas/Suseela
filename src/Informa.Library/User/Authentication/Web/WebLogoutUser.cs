using Jabberwocky.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService]
    public class WebLogoutUser : IWebLogoutUser
    {
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IWebLogoutUserActions Actions;
        IWebAuthenticateUser WebAuthenticateUser;

        public WebLogoutUser(
            IAuthenticatedUserContext authenticatedUserContext,
            IWebLogoutUserActions actions,
            IWebAuthenticateUser webAuthenticateUser)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            Actions = actions;
            WebAuthenticateUser = webAuthenticateUser;
        }

        public void Logout()
        {
            if(WebAuthenticateUser !=null && WebAuthenticateUser.AuthenticatedUser != null )
            {
                WebAuthenticateUser.AuthenticatedUser = null;
            }

            var user = AuthenticatedUserContext.User;

            if (user != null)
                Actions.Process(user);

            AuthenticationManager.Logout();
        }
    }
}
