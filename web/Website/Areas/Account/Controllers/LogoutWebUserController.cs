using Informa.Library.User.Authentication.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.Controllers
{
    public class LogoutWebUserController : Controller
    {
		protected readonly IWebLogoutUser LogoutWebUser;

		public LogoutWebUserController(
			IWebLogoutUser logoutWebUser)
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

        [HttpPost]
        public ActionResult VerticalLogout()
        {
            LogoutWebUser.Logout();
            return new EmptyResult();
        }
    }
}