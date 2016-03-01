using Informa.Library.User.Authentication;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.Controllers
{
    public class LogoutWebUserController : Controller
    {
		protected readonly ILogoutWebUser LogoutWebUser;

		public LogoutWebUserController(
			ILogoutWebUser logoutWebUser)
		{
			LogoutWebUser = logoutWebUser;
		}

		[HttpGet]
		public ActionResult Logout(string returnUrl)
		{
			LogoutWebUser.Logout();

			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return Redirect("/");
		}
    }
}