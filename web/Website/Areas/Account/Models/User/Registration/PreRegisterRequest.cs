using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Registration
{
	public class PreRegisterRequest
	{
		[Required(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		[EmailAddress(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		public string Username { get; set; }
	}
}