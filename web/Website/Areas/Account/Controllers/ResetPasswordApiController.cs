using Informa.Library.User.ResetPassword;
using Informa.Web.Areas.Account.Models.User.ResetPassword;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Linq;

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
		public IHttpActionResult Reset(ResetPasswordRequest request)
		{
			if (!ModelState.IsValid)
			{
				var response = new
				{
					success = false,
					reasons = GetInvalidReasons(ModelState)
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

		public List<string> GetInvalidReasons(ModelStateDictionary modelState)
		{
			var reasons = modelState.SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage)).Distinct().ToList();

			if (!reasons.Any())
			{
				reasons.Add("Unknown");
			}

			return reasons;
		}
	}
}