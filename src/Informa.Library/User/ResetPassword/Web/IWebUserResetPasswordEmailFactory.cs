using Informa.Library.Mail;

namespace Informa.Library.User.ResetPassword.Web
{
	public interface IWebUserResetPasswordEmailFactory
	{
		IEmail Create(IUserResetPassword userResetPassword);
	}
}
