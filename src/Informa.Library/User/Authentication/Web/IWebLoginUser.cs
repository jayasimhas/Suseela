namespace Informa.Library.User.Authentication.Web
{
	public interface IWebLoginUser
	{
		IWebLoginUserResult Login(string username, string password, bool persist);
	}
}
