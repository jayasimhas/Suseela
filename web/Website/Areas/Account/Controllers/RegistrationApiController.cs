using Informa.Library.User.Registration;
using Informa.Library.User.Registration.Web;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Web.Areas.Account.Models.User.Registration;
using System.Web.Http;

namespace Informa.Web.Areas.Account.Controllers
{
	public class RegistrationApiController : ApiController
	{
		protected readonly INewUserFactory NewUserFactory;
		protected readonly IWebRegisterUser RegisterUser;

		public RegistrationApiController(
			INewUserFactory newUserFactory,
			IWebRegisterUser registerUser)
		{
			NewUserFactory = newUserFactory;
			RegisterUser = registerUser;
		}

		[HttpPost]
		[ValidateReasons]
		[ArgumentsRequired]
		public IHttpActionResult Register(RegisterRequest request)
		{
			var newUser = NewUserFactory.Create();

			newUser.FirstName = request.FirstName;
			newUser.LastName = request.LastName;
			newUser.Password = request.Password;
			newUser.Username = request.Username;

			var success = RegisterUser.Register(newUser);

			return Ok(new
			{
				success = success
			});
		}
    }
}
