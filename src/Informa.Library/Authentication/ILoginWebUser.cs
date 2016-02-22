namespace Informa.Library.Authentication
{
	public interface ILoginWebUser
	{
		ILoginWebUserResult Login(string username, string password, bool persist);
	}
}
