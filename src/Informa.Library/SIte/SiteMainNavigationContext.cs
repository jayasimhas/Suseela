using System.Collections.Generic;
using Informa.Library.Navigation;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteMainNavigationContext : ISiteMainNavigationContext
	{
		protected readonly ISitecoreService SitecoreService;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IItemNavigationTreeFactory ItemNavigationTreeFactory;

		public SiteMainNavigationContext(
			ISiteRootContext siteRootContext,
			IItemNavigationTreeFactory itemNavigationTreeFactory,
			ISitecoreService sitecoreService)
		{
			SiteRootContext = siteRootContext;
			ItemNavigationTreeFactory = itemNavigationTreeFactory;
			SitecoreService = sitecoreService;
		}

		public IEnumerable<INavigation> Navigation => ItemNavigationTreeFactory.Create(NavigationRoot);

		public INavigation_Root NavigationRoot => SiteRootContext.Item == null ? null : SitecoreService.GetItem<INavigation_Root>(SiteRootContext.Item.Main_Navigation);
	}
}
