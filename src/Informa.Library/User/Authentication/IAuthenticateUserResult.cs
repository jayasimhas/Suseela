namespace Informa.Library.User.Authentication
{
	public interface IAuthenticateUserResult
	{
		AuthenticateUserResultState State { get; }
		IAuthenticatedUser User { get; }
	}
}
