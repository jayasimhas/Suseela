namespace Informa.Library.User.Authentication
{
	public interface IAuthenticatedUser
	{
		string Username { get; }
		string Name { get; }
		string Email { get; }
	}
}
