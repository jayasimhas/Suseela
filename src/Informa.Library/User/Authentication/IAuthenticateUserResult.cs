namespace Informa.Library.User.Authentication
{
	public interface IAuthenticateUserResult
	{
		AuthenticateUserResultState State { get; }
		IUser User { get; }
	}
}
