namespace Informa.Library.User.Authentication
{
	public interface IAuthenticateUser
	{
		IAuthenticateUserResult Authenticate(string username, string password);
	}
}
