using Informa.Library.Registration;
using Informa.Web.Areas.Account.Models;
using System.Web.Http;

namespace Informa.Web.Areas.Account.Controllers
{
    public class AccountValidationController : ApiController
    {
		protected readonly IUsernameValidator UsernameValidator;

		public AccountValidationController(
			IUsernameValidator usernameValidator)
		{
			UsernameValidator = usernameValidator;
		}

		[HttpPost]
		public IHttpActionResult Username(UsernameRequest request)
		{
			return Ok(new { valid = UsernameValidator.Validate(request.Username) });
		}
	}
}
