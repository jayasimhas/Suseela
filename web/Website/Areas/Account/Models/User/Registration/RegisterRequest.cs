using Informa.Library.ComponentModel.DataAnnotations;
using Informa.Library.User.Registration.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Registration
{
	public class RegisterRequest
	{
		[Required(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		[EmailAddress(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		[CompetitorEmailRestriction(ErrorMessage = RegisterValidationReason.UsernameCompetitorRestriction)]
		[PublicEmailRestriction(ErrorMessage = RegisterValidationReason.UsernamePublicRestriction)]
		public string Username { get; set; }
		[Password(ErrorMessage = RegisterValidationReason.PasswordRequirements)]
		public string Password { get; set; }
		[Compare(nameof(Password), ErrorMessage = RegisterValidationReason.PasswordMismatch)]
		public string PasswordRepeat { get; set; }
		[Required(ErrorMessage = RegisterValidationReason.Required)]
		[MinLength(2, ErrorMessage = RegisterValidationReason.Required)]
		public string FirstName { get; set; }
		[Required(ErrorMessage = RegisterValidationReason.Required)]
		public string LastName { get; set; }
		[MustBeTrue(ErrorMessage = RegisterValidationReason.TermsNotAccepted)]
		public bool TermsAccepted { get; set; }
		public bool AssociateMaster { get; set; }
		[RequiredIf(nameof(AssociateMaster), true, ErrorMessage = RegisterValidationReason.MasterIdInvalid)]
		public string MasterId { get; set; }
		[RequiredIf(nameof(AssociateMaster), true, ErrorMessage = RegisterValidationReason.MasterIdInvalid)]
		public string MasterPassword { get; set; }
	}
}