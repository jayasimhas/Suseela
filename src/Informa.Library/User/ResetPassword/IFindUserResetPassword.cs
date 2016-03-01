namespace Informa.Library.User.ResetPassword
{
	public interface IFindUserResetPassword
	{
		IUserResetPassword Find(string token);
	}
}
