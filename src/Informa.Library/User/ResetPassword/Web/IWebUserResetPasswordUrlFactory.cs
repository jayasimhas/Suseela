namespace Informa.Library.User.ResetPassword.Web
{
	public interface IWebUserResetPasswordUrlFactory
	{
		string Create(IUserResetPassword userResetPassword);
	}
}
