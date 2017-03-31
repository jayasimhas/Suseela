using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Library.Utilities.WebUtils
{
	[AutowireService(LifetimeScope.Default)]
	public class RefreshPage : IRefreshPage
	{
		public void Refresh()
		{
            if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl,true);
		}
	}
}
