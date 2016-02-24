using System.Web.Http;
using Informa.Library.User.Authentication;
using Informa.Web.Areas.Account.Models.User.Authentication;

namespace Informa.Web.Areas.Account.Controllers
{
    public class AuthenticationApiController : ApiController
    {
		protected readonly IAuthenticateUser AuthenticateUser;

		public AuthenticationApiController(
			IAuthenticateUser authenticateUser)
		{
			AuthenticateUser = authenticateUser;
		}

		[HttpPost]
		[HttpGet]
		public IHttpActionResult Authenticate([FromUri]AuthenticateRequest request)
		{
			if (request == null)
			{
				return BadRequest("Request not set");
			}

			var username = request.Username;
			var password = request.Password;

			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				return Ok(new
				{
					success = false
				});
			}

			var result = AuthenticateUser.Authenticate(request.Username, request.Password);

			return Ok(new
			{
				success = result.State == AuthenticateUserResultState.Success,
				id = result.User?.Id,
				username = username
			});
		}
	}
}
