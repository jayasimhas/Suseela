using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebAuthenticateUser : IWebAuthenticateUser
	{
		protected readonly IAuthenticateUser AuthenticateUser;
		protected readonly IWebLoginUser LoginWebUser;

		public WebAuthenticateUser(
			IAuthenticateUser authenticateUser,
			IWebLoginUser loginWebUser)
		{
			AuthenticateUser = authenticateUser;
			LoginWebUser = loginWebUser;
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
