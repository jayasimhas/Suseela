namespace Informa.Library.User.ResetPassword.MongoDB
{
	public interface IUserResetPasswordDocumentFactory
	{
		UserResetPasswordDocument Create(IUserResetPassword userResetPassword);
	}
}
