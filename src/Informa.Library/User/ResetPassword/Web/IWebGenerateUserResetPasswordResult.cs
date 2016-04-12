namespace Informa.Library.User.ResetPassword.Web
{
	public interface IWebGenerateUserResetPasswordResult
	{
		WebGenerateUserResetPasswordStatus Status { get; }
		string Token { get; }
	}
}
