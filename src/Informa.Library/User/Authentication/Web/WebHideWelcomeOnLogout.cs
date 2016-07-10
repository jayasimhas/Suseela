using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Web;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class WebHideWelcomeOnLogout : IWebLogoutUserAction
    {
        //This Class will be responsible for Creating cookie after logout
        // As a Fix of Bug IIS-143
        //Creating Cookie "Loggedout"

        public WebHideWelcomeOnLogout()
        {

        }

        public void Process(IUser value)
        {
            HttpCookie LoggedOutKeyCookie = new HttpCookie("LoggedoutActionFlag");
            LoggedOutKeyCookie.Value = "true";
            LoggedOutKeyCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(LoggedOutKeyCookie);
        }
    }
}