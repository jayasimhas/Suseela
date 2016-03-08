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
		protected readonly IWebSetOptInsRegisterUser SetOptInsRegisterUser;

		public RegistrationApiController(
			INewUserFactory newUserFactory,
			IWebRegisterUser registerUser,
			IWebSetOptInsRegisterUser setOptInsRegisterUser)
		{
			NewUserFactory = newUserFactory;
			RegisterUser = registerUser;
			SetOptInsRegisterUser = setOptInsRegisterUser;
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

		[HttpPost]
		[ArgumentsRequired]
		public IHttpActionResult SetOptIns(SetOptInsRequest request)
		{
			var success = SetOptInsRegisterUser.Set(request.Offers, request.Newsletters);

			return Ok(new
			{
				success = success
			});
		}
    }
}
