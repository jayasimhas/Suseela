namespace Informa.Library.User.ResetPassword
{
	public interface IFindValidUserResetPassword
	{
		IUserResetPassword Find(string token);
	}
}
