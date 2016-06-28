using Informa.Library.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.ResetPassword
{
	public class ChangeRequest
	{
		[Required(ErrorMessage = ChangeValidationReason.MissingToken)]
		public string Token { get; set; }
		[Password(ErrorMessage = ChangeValidationReason.PasswordRequirements)]
		public string NewPassword { get; set; }
		[Compare(nameof(NewPassword), ErrorMessage = ChangeValidationReason.PasswordMismatch)]
		public string NewPasswordRepeat { get; set; }
	}
}