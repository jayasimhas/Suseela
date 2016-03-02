using Informa.Library.User.ResetPassword;
using Informa.Web.Areas.Account.Models.User.ResetPassword;
using System.Web.Http;
using Informa.Library.User.ResetPassword.Web;
using Informa.Library.Utilities.WebApi.Filters;

namespace Informa.Web.Areas.Account.Controllers
{
	public class ResetPasswordApiController : ApiController
	{
		protected readonly IProcessUserResetPassword ProcessUserResetPassword;
		protected readonly IWebGenerateUserResetPassword GenerateUserResetPassword;
		protected readonly IWebRegenerateUserResetPassword RegenerateUserResetPassword;

		public ResetPasswordApiController(
			IProcessUserResetPassword processUserResetPassword,
			IWebGenerateUserResetPassword generateUserResetPassword,
			IWebRegenerateUserResetPassword regenerateUserResetPassword)
		{
			ProcessUserResetPassword = processUserResetPassword;
			GenerateUserResetPassword = generateUserResetPassword;
			RegenerateUserResetPassword = regenerateUserResetPassword;
		}

		[HttpPost]
		[ValidateReasons]
		[ArgumentsRequired]
		public IHttpActionResult Change(ChangeRequest request)
		{
			var token = request.Token;
			var newPassword = request.NewPassword;
			var result = ProcessUserResetPassword.Process(token, newPassword);

			return Ok(new
			{
				success = result.Status == ProcessUserResetPasswordStatus.Success,
			});
		}

		[HttpPost]
		[ArgumentsRequired]
		public IHttpActionResult Retry(RetryRequest request)
		{
			var result = RegenerateUserResetPassword.Regenerate(request?.Token ?? string.Empty);

			return Ok(new
			{
				success = result.Status == WebGenerateUserResetPasswordStatus.Success
			});
		}

		[HttpPost]
		[ValidateReasons]
		[ArgumentsRequired]
		public IHttpActionResult Generate(GenerateRequest request)
		{
			var email = request.Email;
			var result = GenerateUserResetPassword.Generate(email);

			return Ok(new
			{
				success = result.Status == WebGenerateUserResetPasswordStatus.Success
			});
		}
	}
}