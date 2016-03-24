namespace Informa.Library.User.Authentication.Web
{
	public interface IWebAuthenticateUserResult
	{
		AuthenticateUserResultState State { get; set; }
		bool Success { get; set; }
		IUser User { get; set; }
	}
}