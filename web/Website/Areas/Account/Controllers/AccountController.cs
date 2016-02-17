using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Library.CustomSitecore.Mvc;
using Informa.Web.Areas.Account.Models;
using Informa.Web.Authenticator;
using Informa.Web.CustomMvc.Pipelines.HttpRequest;
using Informa.Web.Models;
using Informa.Web.ViewModels;
using Informa.Web.ViewModels.PopOuts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SFRestClient;
using Sitecore.Analytics;

namespace Informa.Web.Controllers
{
    [Authorize]
    public class AccountController : GlassController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private SignInViewModel SignInViewModel;

        public AccountController(ISignInViewModel signInViewModel)
        {
            SignInViewModel = (SignInViewModel)signInViewModel;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ISignInViewModel signInViewModel)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            SignInViewModel = (SignInViewModel)signInViewModel;
        }

        public ApplicationSignInManager SignInManager
        {
            get {
                return _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>());
            }
            private set 
            { 
                _signInManager = value; 
            }
        }
   

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Auth
        [Authorize]
        public override ActionResult Index()
        {
            // Get ID ticket from .ASP.Net cookie. This ticket doesnt contain an identity, 
            // but a reference to the identity in the Session Store                          
            var principal = IdentityHelper.GetCurrentClaimsPrincipal();

            //var ctx = Tracker.Current.Session;
            // Login the sitecore user with the claims identity that was provided by identity ticket
            LoginHelper loginHelper = new LoginHelper();
            loginHelper.Login(principal.Identity);

            //ctx = Tracker.Current.Session;

            return base.Index();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(SignInViewModel);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(SignInViewModel);
        }


        //
        // POST: /Account/Login
        [CustomValidHttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SignInViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }                       

            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);


            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return SignIn(returnUrl);
            }
        }

        //
        // POST: /Account/Login
        [CustomValidHttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            //ApiWebClient restClient = new SFConnector(new OAuth);



            var principal = IdentityHelper.GetCurrentClaimsPrincipal();


            LoginHelper loginHelper = new LoginHelper();
            loginHelper.Login(principal.Identity);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return base.Index();
            }
        }

 
        ////
        // POST: /Account/Register
        [CustomValidHttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterPopOutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Username };
                var result = await UserManager.CreateAsync(user, "Velir123!");
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //         var manager = new UserManager();
                    //var user = new User();
                    //user.Email = email;
                    //user.UserName = username;
                    //user.IsApproved = isApproved;
                    //user.PasswordAnswer = passwordAnswer;
                    //user.PasswordQuestion = passwordQuestion;
                    //// TODO - Connect the OWIN Password Hash
                    //user.PasswordHash = password;
                    //user.Id = providerUserKey == null ? null : providerUserKey.ToString();
                    //var ret = manager.CreateIdentity(user, "Sitecore"); // TODO Change to AppSetting
                    //var id = ret.Name();

                    //status = MembershipCreateStatus.Success;
                    //return GetMembershipUser(manager.Users.FirstOrDefault(i => i.Id == id));

                    return base.Index();
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return base.Index();
        }

     
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToLocal("/");
        }

        //
        // POST: /Account/LogOff      
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToLocal("/");
        }

        //
        // GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        internal class SalesforceConnector
        {
            
        }
        #endregion
    }
}