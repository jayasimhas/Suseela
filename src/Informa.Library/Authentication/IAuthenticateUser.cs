namespace Informa.Library.Authentication
{
	public interface IAuthenticateUser
	{
		IAuthenticateUserResult Authenticate(string username, string password);
	}
}
