namespace Informa.Library.User.Authentication.Web
{
	public interface IWebLogoutUserAction
	{
		void Process(IAuthenticatedUser user);
	}
}
