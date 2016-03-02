using Informa.Library.User.ResetPassword;
using Informa.Web.Areas.Account.Models.User.ResetPassword;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Linq;
using Informa.Library.User.ResetPassword.Web;

namespace Informa.Web.Areas.Account.Controllers
{
	public class ResetPasswordApiController : ApiController
	{
		protected readonly IProcessUserResetPassword ProcessUserResetPassword;
		protected readonly IWebRegenerateUserResetPassword RegenerateUserResetPassword;

		public ResetPasswordApiController(
			IProcessUserResetPassword processUserResetPassword,
			IWebRegenerateUserResetPassword regenerateUserResetPassword)
		{
			ProcessUserResetPassword = processUserResetPassword;
			RegenerateUserResetPassword = regenerateUserResetPassword;
		}

		[HttpPost]
		public IHttpActionResult Change(ChangeRequest request)
		{
			if (!ModelState.IsValid)
			{
				var response = new
				{
					success = false,
					reasons = GetChangeInvalidReasons(ModelState)
				};

				return Ok(response);
			}

			var token = request.Token;
			var newPassword = request.NewPassword;
			var result = ProcessUserResetPassword.Process(token, newPassword);

			return Ok(new
			{
				success = result.Status == ProcessUserResetPasswordStatus.Success,
			});
		}

		[HttpPost]
		public IHttpActionResult Retry(RetryRequest request)
		{
			var result = RegenerateUserResetPassword.Regenerate(request?.Token ?? string.Empty);

			return Ok(new
			{
				success = result.Status == WebGenerateUserResetPasswordStatus.Success
			});
		}

		public List<string> GetChangeInvalidReasons(ModelStateDictionary modelState)
		{
			var reasons = new List<string>();

			if (modelState != null)
			{
				reasons.AddRange(modelState.SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage)).Distinct().ToList());
			}

			if (!reasons.Any())
			{
				reasons.Add("Unknown");
			}

			return reasons;
		}
	}
}