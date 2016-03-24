using System.Web.Http;
using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Informa.Web.Areas.Account.Models.User.Authentication;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.ResetPassword.Web;

namespace Informa.Web.Areas.Account.Controllers
{
    public class LoginWebUserApiController : ApiController
    {
		protected readonly IGenerateUserResetPassword GenerateUserResetPassword;
		protected readonly IWebUserResetPasswordUrlFactory UserResetPasswordUrlFactory;
		protected readonly IWebAuthenticateUser AuthenticateWebUser;

		public LoginWebUserApiController(
			IGenerateUserResetPassword generateUserResetPassword,
			IWebUserResetPasswordUrlFactory userResetPasswordUrlFactory,
			IWebAuthenticateUser authenticateWebUser)
		{
			GenerateUserResetPassword = generateUserResetPassword;
			UserResetPasswordUrlFactory = userResetPasswordUrlFactory;
			AuthenticateWebUser = authenticateWebUser;
		}

		[HttpPost]
		public IHttpActionResult Login(AuthenticateRequest request)
		{
			var username = request?.Username;
			var password = request?.Password;

			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				return Ok(new
				{
					success = false
				});
			}

			var result = AuthenticateWebUser.Authenticate(request.Username, request.Password, request.Persist);
			var redirectUrl = string.Empty;

			if (result.State == AuthenticateUserResultState.TemporaryPassword)
			{
				var userResetPassword = GenerateUserResetPassword.Generate(result.User);

				if (userResetPassword != null)
				{
					redirectUrl = UserResetPasswordUrlFactory.Create(userResetPassword);
				}
			}

			return Ok(new
			{
				success = result.Success,
				redirectUrl = redirectUrl
			});
		}
	}
}
