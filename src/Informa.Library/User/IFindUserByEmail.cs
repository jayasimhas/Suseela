namespace Informa.Library.User
{
	public interface IFindUserByEmail
	{
		IUser Find(string email);
	}
}
