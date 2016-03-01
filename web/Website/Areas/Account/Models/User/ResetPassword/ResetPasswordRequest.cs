using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.ResetPassword
{
	public class ResetPasswordRequest
	{
		[Required(ErrorMessage = ResetPasswordValidationReason.MissingToken)]
		public string Token { get; set; }
		[Required(ErrorMessage = ResetPasswordValidationReason.PasswordRequirements)]
		[MinLength(8, ErrorMessage = ResetPasswordValidationReason.PasswordRequirements)]
		[RegularExpression(@"[^\s]+", ErrorMessage = ResetPasswordValidationReason.PasswordRequirements)]
		public string NewPassword { get; set; }
		[Compare("NewPassword", ErrorMessage = ResetPasswordValidationReason.PasswordMismatch)]
		public string NewPasswordRepeat { get; set; }
	}
}