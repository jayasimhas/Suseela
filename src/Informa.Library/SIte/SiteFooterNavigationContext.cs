using System.Collections.Generic;
using Informa.Library.Navigation;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.Site
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SiteFooterNavigationContext : ISiteFooterNavigationContext
    {
        protected readonly ISitecoreService SitecoreService;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IItemNavigationTreeFactory ItemNavigationTreeFactory;
        public SiteFooterNavigationContext(
             ISiteRootContext siteRootContext,
             IItemNavigationTreeFactory itemNavigationTreeFactory,
             ISitecoreService sitecoreService)
        {
            SiteRootContext = siteRootContext;
            ItemNavigationTreeFactory = itemNavigationTreeFactory;
            SitecoreService = sitecoreService;
        }
        
        public INavigation_Root NavigationMenuOneRoot => SiteRootContext.Item == null ? null : SitecoreService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_1_Navigation);

        public INavigation_Root NavigationMenuTwoRoot => SiteRootContext.Item == null ? null : SitecoreService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_2_Navigation);
    }
}
