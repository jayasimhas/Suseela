namespace Informa.Library.User.ResetPassword
{
	public interface IGenerateUserResetPassword
	{
		IUserResetPassword Generate(IUser user);
	}
}
