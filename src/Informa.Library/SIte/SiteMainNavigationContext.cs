using System.Collections.Generic;
using Informa.Library.Navigation;
using Glass.Mapper.Sc;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SiteMainNavigationContext : ISiteMainNavigationContext
	{
		protected readonly IGlobalSitecoreService GlobalService;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IItemNavigationTreeFactory ItemNavigationTreeFactory;

		public SiteMainNavigationContext(
			ISiteRootContext siteRootContext,
			IItemNavigationTreeFactory itemNavigationTreeFactory,
			IGlobalSitecoreService globalService)
		{
			SiteRootContext = siteRootContext;
			ItemNavigationTreeFactory = itemNavigationTreeFactory;
			GlobalService = globalService;
		}

		public IEnumerable<INavigation> Navigation => ItemNavigationTreeFactory.Create(NavigationRoot);

		public INavigation_Root NavigationRoot => SiteRootContext.Item == null ? null : GlobalService.GetItem<INavigation_Root>(SiteRootContext.Item.Main_Navigation);
	}
}
