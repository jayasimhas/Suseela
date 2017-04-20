namespace Informa.Library.User
{
	public interface IUser
	{
		string Username { get; }

        string UserId { get; }
        string AccessToken { get; }
    }
}
