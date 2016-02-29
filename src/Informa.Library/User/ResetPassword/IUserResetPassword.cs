using System;

namespace Informa.Library.User.ResetPassword
{
	public interface IUserResetPassword
	{
		string Token { get; }
		string Username { get; }
		DateTime Expiration { get; }
	}
}
