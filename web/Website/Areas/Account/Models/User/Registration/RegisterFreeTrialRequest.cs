using Informa.Library.ComponentModel.DataAnnotations;
using Informa.Library.User.Registration.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using Informa.Web.Areas.Account.Models.User.Management;

namespace Informa.Web.Areas.Account.Models.User.Registration
{
	public class RegisterFreeTrialRequest {
        
        [Required(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
        [EmailAddress(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
        [CompetitorEmailRestriction(ErrorMessage = RegisterValidationReason.UsernameCompetitorRestriction)]
        [PublicEmailRestriction(ErrorMessage = RegisterValidationReason.UsernamePublicRestriction)]
        public string Username { get; set; }
		
        [Required(ErrorMessage = RegisterValidationReason.Required)]
        [MinLength(2, ErrorMessage = RegisterValidationReason.Required)]
        public string FirstName { get; set; }
		
        [Required(ErrorMessage = RegisterValidationReason.Required)]
        public string LastName { get; set; }
		
        [Password(ErrorMessage = RegisterValidationReason.PasswordRequirements)]
        public string Password { get; set; }
		
        [Compare(nameof(Password), ErrorMessage = RegisterValidationReason.PasswordMismatch)]
        public string PasswordRepeat { get; set; }
		
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Company { get; set; }
        
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string JobTitle { get; set; }
        
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string PostalCode { get; set; }
        
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Country { get; set; }
        
        [Required(ErrorMessage = ManagementValidationReasons.Required)]
        public string Phone { get; set; }
        
        [MustBeTrue(ErrorMessage = RegisterValidationReason.TermsNotAccepted)]
        public bool TermsAccepted { get; set; }
    }
}