using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Configuration;
using Sitecore.Web;

namespace Elsevier.Web.VWB
{
	public partial class login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Redirect(WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage));
		}
	}
}