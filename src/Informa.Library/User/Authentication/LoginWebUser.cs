using Jabberwocky.Glass.Autofac.Attributes;

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
			
			return new LoginWebUserResult
			{
				Success = result.State == AuthenticateUserResultState.Success,
				Message = string.Format("ID = {0}", result.User?.Id ?? "NULL")
			};
		}
	}
}
