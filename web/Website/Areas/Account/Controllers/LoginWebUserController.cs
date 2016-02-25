using Informa.Library.User.Authentication;
using Informa.Web.ViewModels;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.Controllers
{
    public class LoginWebUserController : Controller
    {
		protected readonly ILoginWebUser LoginWebUser;
		protected readonly SignInViewModel SignInViewModel;

		public LoginWebUserController(
			ILoginWebUser loginWebUser,
			ISignInViewModel signInViewModel)
		{
			LoginWebUser = loginWebUser;
			SignInViewModel = (SignInViewModel)signInViewModel;
		}

		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(SignInViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = LoginWebUser.Login(model.Email, model.Password, model.RememberMe);

			if (result.Success)
			{
				return RedirectToLocal(returnUrl);
			}

			ModelState.AddModelError("", "Invalid login attempt.");
			ViewBag.ReturnUrl = returnUrl;

			return View(SignInViewModel);
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return Redirect("/");
		}
	}
}