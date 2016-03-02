using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.ResetPassword
{
	public class GenerateRequest
	{
		[Required(ErrorMessage = GenerateValidationReason.EmailRequirement)]
		[EmailAddress(ErrorMessage = GenerateValidationReason.EmailRequirement)]
		public string Email { get; set; }
	}
}