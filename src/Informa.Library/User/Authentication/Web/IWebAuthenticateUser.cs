namespace Informa.Library.User.Authentication.Web
{
	public interface IWebAuthenticateUser
	{
		IWebAuthenticateUserResult Authenticate(string username, string password, bool persist);
        IWebAuthenticateUserResult Authenticate(string code, string redirectUrl);
        IAuthenticatedUser AuthenticatedUser { get; set; }
	}
}