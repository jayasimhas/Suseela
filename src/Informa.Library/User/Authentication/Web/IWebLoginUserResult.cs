namespace Informa.Library.User.Authentication.Web
{
	public interface IWebLoginUserResult : IAuthenticateUserResult
	{
		bool Success { get; }
	}
}
