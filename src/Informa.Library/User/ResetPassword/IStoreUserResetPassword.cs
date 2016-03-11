namespace Informa.Library.User.ResetPassword
{
	public interface IStoreUserResetPassword
	{
		bool Store(IUserResetPassword userResetPassword);
	}
}
