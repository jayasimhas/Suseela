namespace Informa.Library.User
{
	public interface IUpdateUserPassword
	{
		bool Update(IUser user, string newPassword);
	}
}
