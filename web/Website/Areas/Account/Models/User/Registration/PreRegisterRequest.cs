using Informa.Library.User.Registration.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Registration
{
	public class PreRegisterRequest
	{
		[Required(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		[EmailAddress(ErrorMessage = RegisterValidationReason.UsernameRequirements)]
		[CompetitorEmailRestriction(ErrorMessage = RegisterValidationReason.UsernameCompetitorRestriction)]
		[PublicEmailRestriction(ErrorMessage = RegisterValidationReason.UsernamePublicRestriction)]
		public string Username { get; set; }
	}
}