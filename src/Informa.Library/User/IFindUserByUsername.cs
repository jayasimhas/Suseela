namespace Informa.Library.User
{
	public interface IFindUserByUsername
	{
		IUser Find(string username);
	}
}
