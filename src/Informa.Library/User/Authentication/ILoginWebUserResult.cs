namespace Informa.Library.User.Authentication
{
	public interface ILoginWebUserResult
	{
		bool Success { get; }
		string Message { get; }
	}
}
