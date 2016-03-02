namespace Informa.Library.User.ResetPassword
{
	public interface IProcessUserResetPassword
	{
		IProcessUserResetPasswordResult Process(string token, string newPassword);
	}
}
