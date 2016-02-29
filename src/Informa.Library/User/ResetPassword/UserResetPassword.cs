using System;

namespace Informa.Library.User.ResetPassword
{
	public class UserResetPassword : IUserResetPassword
	{
		public string Token { get; set; }
		public string Username { get; set; }
		public DateTime Expiration { get; set; }
		public string Name { get; set; }
	}
}
