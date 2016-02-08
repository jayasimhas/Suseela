using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Informa.Web.Authenticator;
using Informa.Web.CustomMvc.Pipelines.HttpRequest;
using Informa.Web.ViewModels;
using Informa.Web.ViewModels.PopOuts;
using Microsoft.Owin.Security;
using Sitecore.Analytics;
using Sitecore.Security.Authentication;

namespace Informa.Web.Controllers
{
    public class Auth2Controller : Controller
    {
        public ISignInViewModel SignInViewModel { get; set; }
        public Auth2Controller(ISignInViewModel signInViewModel)
        {
            SignInViewModel = signInViewModel;
        }

                                          
        public ActionResult Index()
        {
            // Get ID ticket from .ASP.Net cookie. This ticket doesnt contain an identity, 
            // but a reference to the identity in the Session Store                          
            var principal = IdentityHelper.GetCurrentClaimsPrincipal();

            var ctx = Tracker.Current.Session;
            // Login the sitecore user with the claims identity that was provided by identity ticket
            LoginHelper loginHelper = new LoginHelper();
            loginHelper.Login(principal.Identity);

            ctx = Tracker.Current.Session;

            return View();


        // GET: Auth
        //public ActionResult Index()
        //{
            //var domain = Sitecore.Context.Domain;

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


            //return View();
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
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.RedirectUri = "/login";
            properties.AllowRefresh = false;
            AuthenticationManager.Logout();
            Request.GetOwinContext().Authentication.SignOut(properties);
            return new EmptyResult();
        }
    }
}