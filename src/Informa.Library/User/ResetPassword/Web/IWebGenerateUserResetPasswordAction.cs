namespace Informa.Library.User.ResetPassword.Web
{
	public interface IWebGenerateUserResetPasswordAction
	{
		void Process(IUserResetPassword userResetPassword);
	}
}
