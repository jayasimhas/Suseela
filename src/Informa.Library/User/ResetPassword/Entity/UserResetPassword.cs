using System;
using System.ComponentModel.DataAnnotations;

namespace Informa.Library.User.ResetPassword.Entity
{
	public class UserResetPassword : IUserResetPassword
	{
		[Key]
		public string Token { get; set; }
		public string Username { get; set; }
		public DateTime Expiration { get; set; }
	}
}
