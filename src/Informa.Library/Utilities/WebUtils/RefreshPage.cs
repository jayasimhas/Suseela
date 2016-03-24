using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Library.Utilities.WebUtils
{
	[AutowireService(LifetimeScope.Default)]
	public class RefreshPage : IRefreshPage
	{
		public void Refresh()
		{
			HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
		}
	}
}
