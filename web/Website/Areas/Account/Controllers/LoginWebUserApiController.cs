using System.Web.Http;
using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Informa.Web.Areas.Account.Models.User.Authentication;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.ResetPassword.Web;
using Informa.Library.User.UserPreference;
using System.Web;
using Informa.Library.Salesforce.User.Authentication;
using Informa.Library.User;
using System.Configuration;

namespace Informa.Web.Areas.Account.Controllers
{
    public class LoginWebUserApiController : ApiController
    {
        protected readonly IGenerateUserResetPassword GenerateUserResetPassword;
        protected readonly IWebUserResetPasswordUrlFactory UserResetPasswordUrlFactory;
        protected readonly IWebAuthenticateUser AuthenticateWebUser;
        protected readonly IMyViewToggleRedirectUrlFactory MyViewRedirectUrlFactory;
        protected readonly ISitecoreUserContext SitecoreUserContext;

        public LoginWebUserApiController(
            IGenerateUserResetPassword generateUserResetPassword,
            IWebUserResetPasswordUrlFactory userResetPasswordUrlFactory,
            IWebAuthenticateUser authenticateWebUser,
            IMyViewToggleRedirectUrlFactory myViewRedirectUrlFactory, ISitecoreUserContext sitecoreUserContext)
        {
            GenerateUserResetPassword = generateUserResetPassword;
            UserResetPasswordUrlFactory = userResetPasswordUrlFactory;
            AuthenticateWebUser = authenticateWebUser;
            MyViewRedirectUrlFactory = myViewRedirectUrlFactory;
            SitecoreUserContext = sitecoreUserContext;
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
            string curVertical = HttpContext.Current.Request.QueryString["vid"] ?? HttpContext.Current.Request.QueryString["vid"];

            var result = AuthenticateWebUser.Authenticate(request.Username, request.Password, request.Persist, curVertical);
            var redirectUrl = string.Empty;

            if (request.IsSignInFromMyView)
            {
                redirectUrl = MyViewRedirectUrlFactory.create();
            }
            if (result.State == AuthenticateUserResultState.TemporaryPassword)
            {
                var userResetPassword = GenerateUserResetPassword.Generate(result.User);

                if (userResetPassword != null)
                {
                    redirectUrl = UserResetPasswordUrlFactory.Create(userResetPassword);
                }
            }
            //Current vertical cookiename
            string cookieName = curVertical + "_LoggedInUser";
            //Current Vertical subdomain
            string domain = ConfigurationManager.AppSettings[curVertical];

            HttpCookie LoggedinKeyCookie = new HttpCookie(curVertical+"_LoggedInUser");
            LoggedinKeyCookie.Value = username;
            LoggedinKeyCookie.Expires = System.DateTime.Now.AddDays(1);
            LoggedinKeyCookie.Domain = domain;
            HttpContext.Current.Response.Cookies.Add(LoggedinKeyCookie);

            
            return Ok(new
            {
                success = result.Success,
                redirectUrl = redirectUrl,
                RedirectRequired = (request.IsSignInFromMyView && !string.IsNullOrWhiteSpace(redirectUrl)) ? true : false
            });
        }
       
        [HttpPost]
        public IHttpActionResult VerticalLogin(AuthenticateRequest request)
        {
            var userContext = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthenticatedUserContext)) as IAuthenticatedUserContext;
            if (!string.IsNullOrEmpty(request.Username))
            {
                IAuthenticatedUser authenticatedUser = new AuthenticatedUser() { Username = request.Username };
                var result = AuthenticateWebUser.Authenticate(authenticatedUser);
                return Ok(new
                {
                    success = result.Success,
                });
            }
            return Ok(new
            {
                success = "false",
            });
        }
    }
}
