using Informa.Library.User;

namespace Informa.Library.Authentication
{
	public interface IAuthenticateUserResult
	{
		AuthenticateUserResultState State { get; }
		IUser User { get; }
	}
}
