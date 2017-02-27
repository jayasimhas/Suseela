using Sitecore.Configuration;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Informa.Web.VWB.Util
{
    public partial class LoginRedirectToCMS : System.Web.UI.Page
    {
        private Sitecore.Security.Accounts.User _user;
        protected void Page_Init(object sender, EventArgs e)
        {
            _user = Sitecore.Context.User;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string redirect = Request["redirect"];
            //string redirectUrl = redirect + "Sitecore.Context.ClientPage.SendMessage(this, redirect)";
            if (!_user.IsAuthenticated)
            {
                string url = WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage) + "?redirect=" + HttpUtility.UrlEncode(redirect);
                Response.Redirect(url);
            }
            else
            {
                Sitecore.Context.ClientPage.SendMessage(this, redirect);
                Response.Redirect(redirect);
            }
        }
    }
}