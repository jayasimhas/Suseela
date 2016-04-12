namespace Informa.Library.User.Authentication.Web
{
	public interface IWebLoginUser
	{
		IWebLoginUserResult Login(IUser user, bool persist);
	}
}
