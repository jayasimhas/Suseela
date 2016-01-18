using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly ISiteHomeContext SiteHomeContext;
		protected readonly ISiteRootContext SiteRootContext;

		public HeaderViewModel(
			ISiteHomeContext siteHomeContext,
			ISiteRootContext siteRootContext)
		{
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
		}

		public string LogoImageUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Site_Logo.Src;

		public string LogoUrl => SiteHomeContext.Item == null ? string.Empty : SiteHomeContext.Item._Url;
	}
}