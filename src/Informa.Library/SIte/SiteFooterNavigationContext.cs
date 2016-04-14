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

        //public IEnumerable<INavigation> FooterMenuOneNavigation => ItemNavigationTreeFactory.Create(NavigationMenuOneRoot).Any() ? ItemNavigationTreeFactory.Create(NavigationMenuOneRoot).FirstOrDefault().Children : Enumerable.Empty<INavigation>();
        ////new System.Collections.Generic.Mscorlib_CollectionDebugView<Informa.Library.Navigation.Navigation>(ItemNavigationTreeFactory.Create(SitecoreService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_1_Navigation))).Items[0].Children

        //public IEnumerable<INavigation> FooterMenuTwoNavigation => ItemNavigationTreeFactory.Create(NavigationMenuTwoRoot).Any() ? ItemNavigationTreeFactory.Create(NavigationMenuTwoRoot).FirstOrDefault().Children : Enumerable.Empty<INavigation>();
        ////public IEnumerable<INavigation> FooterMenuTwoNavigation => ItemNavigationTreeFactory.Create(NavigationMenuTwoRoot).FirstOrDefault().Children;

        public INavigation_Root NavigationMenuOneRoot => SiteRootContext.Item == null ? null : SitecoreService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_1_Navigation);

        public INavigation_Root NavigationMenuTwoRoot => SiteRootContext.Item == null ? null : SitecoreService.GetItem<INavigation_Root>(SiteRootContext.Item.Footer_Menu_2_Navigation);
    }
}
