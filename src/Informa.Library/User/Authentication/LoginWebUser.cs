using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.Default)]
	public class LoginWebUser : ILoginWebUser
	{
		protected IAuthenticateUser AuthenticateUser;
		protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;

		public LoginWebUser(
			IAuthenticateUser authenticateUser,
			ISitecoreVirtualUsernameFactory virtualUsernameFactory)
		{
			AuthenticateUser = authenticateUser;
			VirtualUsernameFactory = virtualUsernameFactory;
		}

		public ILoginWebUserResult Login(string username, string password, bool persist)
		{
			var result = AuthenticateUser.Authenticate(username, password);
			var state = result.State;
			var authenticatedUser = result.User;
			var authenticated = state == AuthenticateUserResultState.Success;

			if (authenticated)
			{
				var sitecoreUsername = VirtualUsernameFactory.Create(authenticatedUser);
				var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, persist);

				sitecoreVirtualUser.Profile.Email = authenticatedUser.Email;
				sitecoreVirtualUser.Profile.Name = authenticatedUser.Name;

				AuthenticationManager.LoginVirtualUser(sitecoreVirtualUser);
			}

			return new LoginWebUserResult
			{
				State = state,
				Success = authenticated,
				User = authenticatedUser
			};
		}
	}
}
