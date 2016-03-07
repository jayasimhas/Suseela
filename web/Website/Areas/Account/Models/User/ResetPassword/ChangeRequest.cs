using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.ResetPassword
{
	public class ChangeRequest
	{
		[Required(ErrorMessage = ChangeValidationReason.MissingToken)]
		public string Token { get; set; }
		[Required(ErrorMessage = ChangeValidationReason.PasswordRequirements)]
		[MinLength(8, ErrorMessage = ChangeValidationReason.PasswordRequirements)]
		[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = ChangeValidationReason.PasswordRequirements)]
		public string NewPassword { get; set; }
		[Compare("NewPassword", ErrorMessage = ChangeValidationReason.PasswordMismatch)]
		public string NewPasswordRepeat { get; set; }
	}
}