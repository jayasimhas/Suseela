using System;

namespace Informa.Library.User.Authentication
{
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

			throw new NotImplementedException();
		}
	}
}
