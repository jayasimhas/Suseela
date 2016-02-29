namespace Informa.Library.User.ResetPassword
{
	public interface IUserResetPasswordFactory
	{
		IUserResetPassword Create(IUser user);
	}
}
