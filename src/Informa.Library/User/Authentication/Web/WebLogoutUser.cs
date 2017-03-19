using Jabberwocky.Autofac.Attributes;
using Sitecore.Security.Authentication;
using System.Configuration;
using System.Web;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService]
    public class WebLogoutUser : IWebLogoutUser
    {
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IWebLogoutUserActions Actions;
        protected readonly IverticalLogin VerticalLogin;
        public WebLogoutUser(
            IAuthenticatedUserContext authenticatedUserContext,
            IWebLogoutUserActions actions, IverticalLogin verticalLogin)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            Actions = actions;
            VerticalLogin = verticalLogin;
        }

        public void Logout()
        {
            var user = AuthenticatedUserContext.User;

            if (user != null)
                Actions.Process(user);

            AuthenticationManager.Logout();

            //string curVertical = HttpContext.Current.Request.QueryString["vid"] ?? HttpContext.Current.Request.QueryString["vid"];
            ////Current vertical cookiename
            //string cookieName = curVertical + "_LoggedInUser";
            //if (HttpContext.Current.Request.Cookies[cookieName] != null)
            //{
            //    //Current Vertical subdomain
            //    string domain = ConfigurationManager.AppSettings[curVertical];
            //    HttpCookie LoggedinKeyCookie = HttpContext.Current.Request.Cookies[cookieName];
            //    LoggedinKeyCookie.Value = "";
            //    LoggedinKeyCookie.Expires = System.DateTime.Now.AddDays(-1);
            //    LoggedinKeyCookie.Domain = domain;
            //    LoggedinKeyCookie.Path = "/";
            //    HttpContext.Current.Response.Cookies.Add(LoggedinKeyCookie);
            //}
            VerticalLogin.DeleteLoginCookie();
        }
    }
}
