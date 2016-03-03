namespace Informa.Library.User.Authentication
{
	public interface ILoginWebUserResult : IAuthenticateUserResult
	{
		bool Success { get; }
	}
}
