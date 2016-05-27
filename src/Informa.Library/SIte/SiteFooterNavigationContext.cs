using System.Collections.Generic;
using Informa.Library.Navigation;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;
using Informa.Library.Services.Global;

namespace Informa.Library.Site
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SiteFooterNavigationContext : ISiteFooterNavigationContext
    {
        protected readonly IGlobalService GlobalService;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IItemNavigationTreeFactory ItemNavigationTreeFactory;
        public SiteFooterNavigationContext(
             ISiteRootContext siteRootContext,
             IItemNavigationTreeFactory itemNavigationTreeFactory,
             IGlobalService globalService)
        {
            SiteRootContext = siteRootContext;
            ItemNavigationTreeFactory = itemNavigationTreeFactory;
            GlobalService = globalService;
        }
        
        public INavigation_Root NavigationMenuOneRoot => SiteRootContext.Item == null ? null : GlobalService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_1_Navigation);

        public INavigation_Root NavigationMenuTwoRoot => SiteRootContext.Item == null ? null : GlobalService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_2_Navigation);
    }
}
