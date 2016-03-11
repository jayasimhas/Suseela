namespace Informa.Library.User.ResetPassword.Web
{
	public interface IWebRegenerateUserResetPassword
	{
		IWebGenerateUserResetPasswordResult Regenerate(string token);
	}
}
