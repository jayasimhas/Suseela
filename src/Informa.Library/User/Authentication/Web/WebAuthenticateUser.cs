using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebAuthenticateUser : IWebAuthenticateUser
	{
		protected readonly IAuthenticateUser AuthenticateUser;
		protected readonly IWebLoginUser LoginWebUser;
		protected readonly IUserSession UserSession;
		private const string sessionKey = nameof(WebAuthenticateUser);
		public WebAuthenticateUser(
			IAuthenticateUser authenticateUser,
			IWebLoginUser loginWebUser,
			IUserSession userSession)
		{
			AuthenticateUser = authenticateUser;
			LoginWebUser = loginWebUser;
			UserSession = userSession;
		}

		public IAuthenticatedUser AuthenticatedUser
		{
			get
			{
				var authenticatedUserSession = UserSession.Get<IAuthenticatedUser>(sessionKey);

				if (authenticatedUserSession.HasValue)
				{
					return authenticatedUserSession.Value;
				}
				return null;

			}
			set { UserSession.Set(sessionKey, value); }
		}


		public IWebAuthenticateUserResult Authenticate(string username, string password, bool persist)
		{
			var authenticateResult = AuthenticateUser.Authenticate(username, password);
			var state = authenticateResult.State;

			var authenticatedUser = authenticateResult.User;
			var success = state == AuthenticateUserResultState.Success;

			if (success)
			{
				var loginResult = LoginWebUser.Login(authenticatedUser, persist);
				success = loginResult.Success;
				AuthenticatedUser = authenticatedUser;
			}

			return new WebAuthenticateUserResult
			{
				State = state,
				Success = success,
				User = authenticatedUser
			};
		}
	}
}
