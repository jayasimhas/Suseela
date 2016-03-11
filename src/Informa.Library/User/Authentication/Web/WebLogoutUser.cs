using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.Default)]
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
			AuthenticationManager.Logout();

			var user = AuthenticatedUserContext.User;

			Actions.Process(user);
		}
	}
}
