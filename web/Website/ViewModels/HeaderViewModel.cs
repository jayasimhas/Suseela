using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly ISiteRootContext SiteRootContext;

		public HeaderViewModel(
			ISiteRootContext siteRootContext)
		{
			SiteRootContext = siteRootContext;
		}

		public string LogoUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Site_Logo.Src;
	}
}