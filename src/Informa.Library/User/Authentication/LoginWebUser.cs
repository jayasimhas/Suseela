using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Analytics;
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
			var authenticatedUser = result.User;
			var authenticated = result.State == AuthenticateUserResultState.Success;

			if (authenticated)
			{
				var sitecoreUsername = VirtualUsernameFactory.Create(authenticatedUser);
				var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, persist);

				sitecoreVirtualUser.Profile.Email = authenticatedUser.Email;
				sitecoreVirtualUser.Profile.Name = authenticatedUser.Name;

				AuthenticationManager.LoginVirtualUser(sitecoreVirtualUser);

				var tracker = Tracker.Current;

				if (tracker != null)
				{
					tracker.Session.Identify(authenticatedUser.Username);
				}
			}

			return new LoginWebUserResult
			{
				Success = authenticated,
				Message = string.Empty
			};
		}
	}
}
