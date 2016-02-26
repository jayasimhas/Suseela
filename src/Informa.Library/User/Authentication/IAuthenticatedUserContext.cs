namespace Informa.Library.User.Authentication
{
	public interface IAuthenticatedUserContext
	{
		IAuthenticatedUser User { get; }
		bool IsAuthenticated { get; }
	}
}
