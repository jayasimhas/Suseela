using Informa.Library.User.ResetPassword;
using Informa.Web.Areas.Account.Models.User.ResetPassword;
using System.Web.Http;

namespace Informa.Web.Areas.Account.Controllers
{
	public class ResetPasswordApiController : ApiController
	{
		protected readonly IProcessUserResetPassword ProcessUserResetPassword;

		public ResetPasswordApiController(
			IProcessUserResetPassword processUserResetPassword)
		{
			ProcessUserResetPassword = processUserResetPassword;
		}

		[HttpPost]
		[HttpGet]
		public IHttpActionResult Reset([FromUri]ResetPasswordRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = request.Token;
			var newPassword = request.NewPassword;
			var result = ProcessUserResetPassword.Process(token, newPassword);

			return Ok(new
			{
				success = result.Status == ProcessUserResetPasswordStatus.Success,
			});
		}
	}
}