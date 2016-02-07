using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Informa.Web.Authenticator;
using Informa.Web.CustomMvc.Pipelines.HttpRequest;
using Informa.Web.ViewModels;
using Informa.Web.ViewModels.PopOuts;

namespace Informa.Web.Controllers
{
    public class AuthController : Controller
    {
        public ISignInViewModel SignInViewModel { get; set; }
        public AuthController(ISignInViewModel signInViewModel)
        {
            SignInViewModel = signInViewModel;
        }                                                                

        // GET: Auth
        public ActionResult Index()
        {
            var domain = Sitecore.Context.Domain;

            //var properties = new AuthenticationProperties();
            //System.Web.HttpContext.Current.GetOwinContext().Authentication.Challenge();


            //var principal = IdentityHelper.GetCurrentClaimsPrincipal();

            //// Login the sitecore user with the claims identity that was provided by identity ticket
            //LoginHelper loginHelper = new LoginHelper();
            //loginHelper.Login(principal);
            //else
            //{
            //    var returnUrl = HttpUtility.ParseQueryString(ctx.QueryString.ToString()).Get("returnUrl");
            //    if (returnUrl.Contains("sitecore/shell"))
            //        returnUrl = StartUrl;
            //    //WriteCookie("sitecore_starturl", StartUrl);
            //    //WriteCookie("sitecore_starttab", "advanced");
            //    Response.Redirect(returnUrl);


            return View();
        }

        public ActionResult Login()
        {
            return View(SignInViewModel);
        }

        [HttpPost]
        public ActionResult Login(ISignInViewModel signInModel)
        {
            return new EmptyResult();
        }

        public ActionResult Logout()
        {
            return View();
        }
    }
}