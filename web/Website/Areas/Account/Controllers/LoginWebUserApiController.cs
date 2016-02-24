using System.Web.Http;
using Informa.Library.User.Authentication;
using Informa.Web.Areas.Account.Models.User.Authentication;

namespace Informa.Web.Areas.Account.Controllers
{
    public class LoginWebUserApiController : ApiController
    {
		protected readonly ILoginWebUser LoginWebUser;

		public LoginWebUserApiController(
			ILoginWebUser loginWebUser)
		{
			LoginWebUser = loginWebUser;
		}

		[HttpPost]
		[HttpGet]
		public IHttpActionResult Login([FromUri]AuthenticateRequest request)
		{
			var username = request?.Username;
			var password = request?.Password;

			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				return Ok(new
				{
					success = false,
					message = "Username or password missing"
				});
			}

			var result = LoginWebUser.Login(request.Username, request.Password, request.Persist);

			return Ok(new
			{
				success = result.Success,
				message = result.Message,
				username = username
			});
		}
	}
}
