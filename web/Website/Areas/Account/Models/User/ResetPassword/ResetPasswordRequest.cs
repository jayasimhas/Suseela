using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.ResetPassword
{
	public class ResetPasswordRequest
	{
		[Required]
		public string Token { get; set; }
		[Required]
		public string NewPassword { get; set; }
		[Required]
		[Compare("NewPassword")]
		public string NewPasswordRepeat { get; set; }
	}
}