using System.Web;
using Sitecore.Pipelines.LoggedIn;
using Sitecore.Web;

namespace Informa.Library.CustomSitecore.Pipelines
{
	public class LoginRedirect : LoggedInProcessor
	{
		public override void Process(LoggedInArgs args)
		{
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
		}
	}
}