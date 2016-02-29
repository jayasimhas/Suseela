namespace Informa.Library.User.Authentication
{
	public interface IAuthenticatedUser : IUser
	{
		string Name { get; }
		string Email { get; }
	}
}
