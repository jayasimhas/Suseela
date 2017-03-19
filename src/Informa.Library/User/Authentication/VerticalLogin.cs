using Informa.Library.Services.Global;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Informa.Library.User.Authentication
{
    [AutowireService(LifetimeScope.Default)]
    public class VerticalLogin : IverticalLogin
    {
        protected readonly ISitecoreUserContext SitecoreUserContext;
        protected readonly IGlobalSitecoreService GlobalService;
        public string _curVertical = string.Empty;


        public VerticalLogin(ISitecoreUserContext sitecoreUserContext, IGlobalSitecoreService globalService)
        {
            SitecoreUserContext = sitecoreUserContext;
            GlobalService = globalService;
            curVertical = HttpContext.Current.Request.QueryString["vid"] ?? HttpContext.Current.Request.QueryString["vid"];
        }

        public string curVertical
        {
            set
            {
                this._curVertical = value;
            }
            get
            {
                return this._curVertical;
            }
        }
       
        public void CreateLoginCookie(string userName, string userToken)
        {
            //Current vertical cookiename
            string cookieName = GetVerticalCookieName();
            //Current Vertical subdomain
            string domain = GetLoginCookieSubdomain();
            string cookieValue = string.IsNullOrEmpty(userToken) ? userName : userName + "|" + userToken;
            HttpCookie LoggedinKeyCookie = HttpContext.Current.Request.Cookies[cookieName] != null ? HttpContext.Current.Request.Cookies[cookieName] : new HttpCookie(cookieName);
            LoggedinKeyCookie.Value = cookieValue;
            LoggedinKeyCookie.Expires = System.DateTime.Now.AddMinutes(GetCookieTimeOut());
            LoggedinKeyCookie.Domain = domain;
            HttpContext.Current.Response.Cookies.Add(LoggedinKeyCookie);
        }

        public void DeleteLoginCookie()
        {
            //Current vertical cookiename
            string cookieName = GetVerticalCookieName();
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                //Current Vertical subdomain
                string domain = GetLoginCookieSubdomain();
                HttpCookie LoggedinKeyCookie = HttpContext.Current.Request.Cookies[cookieName];
                LoggedinKeyCookie.Value = "";
                LoggedinKeyCookie.Expires = DateTime.Now.AddDays(-1);
                LoggedinKeyCookie.Domain = domain;
                //LoggedinKeyCookie.Path = "/";
                HttpContext.Current.Response.Cookies.Add(LoggedinKeyCookie);
            }
        }

        public string GetVerticalCookieName()
        {
            if (string.IsNullOrEmpty(curVertical))
                return " _LoggedInUser";
            else
            {
                string ssoVerticals = ConfigurationManager.AppSettings["SSOVerticals"];
                if (ssoVerticals.Contains(curVertical))
                    return "SSO_LoggedInUser";
                else
                    return curVertical + "_LoggedInUser";
            }

        }

       public int GetCookieTimeOut()
        {
            if (string.IsNullOrEmpty(curVertical))
                return 480;
            else
            {
                int timeout = 480;
                string ssoVerticals = ConfigurationManager.AppSettings["SSOVerticals"];
                if(ssoVerticals.Contains(curVertical))
                {
                  string timeoutInstr = ConfigurationManager.AppSettings["SSOTimeout"];
                    if (int.TryParse(timeoutInstr, out timeout))
                        return timeout;
                    else
                        return timeout;
                }
                else
                {
                    string timeoutInstr = ConfigurationManager.AppSettings[curVertical+"Timeout"];
                    if (int.TryParse(timeoutInstr, out timeout))
                        return timeout;
                    else
                        return timeout;
                }
            }
        }

        public string GetLoginCookieSubdomain()
        {
            if (string.IsNullOrEmpty(curVertical))
                return "informa.com";
            else
            {
                string ssoVerticals = ConfigurationManager.AppSettings["SSOVerticals"];
                if (ssoVerticals.Contains(curVertical))
                {
                    string subDomain = ConfigurationManager.AppSettings["SSOSubdomain"];
                    return subDomain;
                }
                else
                {
                    string subDomain = ConfigurationManager.AppSettings[curVertical + "Subdomain"];
                    return subDomain;
                }
            }
        }

    }
}
