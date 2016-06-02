using Informa.Library.Navigation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.Services.Global;

namespace Informa.Library.Site
{
    [AutowireService(LifetimeScope.PerScope)]
    public class SiteFooterNavigationContext : ISiteFooterNavigationContext
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IItemNavigationTreeFactory ItemNavigationTreeFactory;
        public SiteFooterNavigationContext(
             ISiteRootContext siteRootContext,
             IItemNavigationTreeFactory itemNavigationTreeFactory,
             IGlobalSitecoreService globalService)
        {
            SiteRootContext = siteRootContext;
            ItemNavigationTreeFactory = itemNavigationTreeFactory;
            GlobalService = globalService;
        }
        
        public INavigation_Root NavigationMenuOneRoot => SiteRootContext.Item == null ? null : GlobalService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_1_Navigation);

        public INavigation_Root NavigationMenuTwoRoot => SiteRootContext.Item == null ? null : GlobalService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_2_Navigation);
    }
}
