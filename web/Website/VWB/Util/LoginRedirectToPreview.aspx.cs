using System;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Security.Accounts;
using Sitecore.Web;

namespace Elsevier.Web.Util
{
	public partial class LoginRedirectToPreview : System.Web.UI.Page
	{
		private User _user;
		protected void Page_Init(object sender, EventArgs e)
		{
			_user = Sitecore.Context.User;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			string redirect = Request["redirect"];
			if (!_user.IsAuthenticated)
			{
				string url = WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage) + "?redirect=" + HttpUtility.UrlEncode(redirect);
				Response.Redirect(url);
			}
			else
			{
				Response.Redirect(redirect);
			}
		}
	}
}