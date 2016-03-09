using Informa.Library.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Registration
{
	public class RegisterRequest
	{
		[Required(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		[EmailAddress(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		public string Username { get; set; }
		[Password(ErrorMessage = RegisterValidationReason.PasswordRequirements)]
		public string Password { get; set; }
		[Compare("Password", ErrorMessage = RegisterValidationReason.PasswordMismatch)]
		public string PasswordRepeat { get; set; }
		[Required(ErrorMessage = RegisterValidationReason.Required)]
		[MinLength(2, ErrorMessage = RegisterValidationReason.Required)]
		public string FirstName { get; set; }
		[Required(ErrorMessage = RegisterValidationReason.Required)]
		public string LastName { get; set; }
		[MustBeTrue(ErrorMessage = RegisterValidationReason.TermsNotAccepted)]
		public bool TermsAccepted { get; set; }
	}
}