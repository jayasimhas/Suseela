using System.Web;
using Sitecore.Pipelines.LoggedIn;
using Sitecore.Web;

namespace Informa.Library.CustomSitecore.Pipelines
{
	public class LoginRedirect : LoggedInProcessor
	{
		public override void Process(LoggedInArgs args)
		{
            Sitecore.Diagnostics.Log.Info("Started LoginRedirect", " LoginRedirect ");
            if (Sitecore.Context.GetSiteName() != "login")
			{
				return;
			}

			string url = HttpUtility.UrlDecode(
				WebUtil.GetQueryString("url", "")
				);
			if (!string.IsNullOrWhiteSpace(url))
			{
				WebUtil.Redirect(url);
			}
            Sitecore.Diagnostics.Log.Info("Ended LoginRedirect", " LoginRedirect");
        }
	}
}