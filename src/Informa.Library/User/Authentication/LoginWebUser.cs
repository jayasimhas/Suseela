using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.Default)]
	public class LoginWebUser : ILoginWebUser
	{
		protected IAuthenticateUser AuthenticateUser;

		public LoginWebUser(
			IAuthenticateUser authenticateUser)
		{
			AuthenticateUser = authenticateUser;
		}

		public ILoginWebUserResult Login(string username, string password, bool persist)
		{
			var result = AuthenticateUser.Authenticate(username, password);
			var authenticated = result.State == AuthenticateUserResultState.Success;

			if (authenticated)
			{
				var sitecoreUsername = string.Format("{0}\\{1}", Context.Domain.Name, username);
				var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, persist);

				sitecoreVirtualUser.Profile.Email = username;

				AuthenticationManager.LoginVirtualUser(sitecoreVirtualUser);

				var tracker = Tracker.Current;

				if (tracker != null)
				{
					tracker.Session.Identify(sitecoreVirtualUser.Identity.Name);
				}
			}

			return new LoginWebUserResult
			{
				Success = authenticated,
				Message = string.Format("ID = {0}", result.User?.Id ?? "NULL")
			};
		}
	}
}
