namespace Informa.Library.User.ResetPassword.Web
{
	public interface IWebGenerateUserResetPassword
	{
		IWebGenerateUserResetPasswordResult Generate(string email);
	}
}
