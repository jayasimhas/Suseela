namespace Informa.Library.User.Authentication.Web
{
	public interface IWebLoginUserAction
	{
		void Process(IAuthenticatedUser authenticatedUser);
	}
}
