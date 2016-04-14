using Informa.Library.Navigation;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.Default)]
    public class FooterNavigationMenuViewModel : IFooterNavigationMenuViewModel
    {
        ISiteFooterNavigationContext _siteFooterNavContext;
        IItemNavigationTreeFactory _itemNavigationTreeFactory;
        public FooterNavigationMenuViewModel(
            ISiteFooterNavigationContext siteFooterNavContext,
            IItemNavigationTreeFactory itemNavigationTreeFactory)
        {
            _siteFooterNavContext = siteFooterNavContext;
            _itemNavigationTreeFactory = itemNavigationTreeFactory;
        }

        public IEnumerable<INavigation> MenuOneNavigation => _itemNavigationTreeFactory.Create(_siteFooterNavContext.NavigationMenuOneRoot).Any() ? _itemNavigationTreeFactory.Create(_siteFooterNavContext.NavigationMenuOneRoot).FirstOrDefault().Children : Enumerable.Empty<INavigation>();

        public IEnumerable<INavigation> MenuTwoNavigation => _itemNavigationTreeFactory.Create(_siteFooterNavContext.NavigationMenuTwoRoot).Any() ? _itemNavigationTreeFactory.Create(_siteFooterNavContext.NavigationMenuTwoRoot).FirstOrDefault().Children : Enumerable.Empty<INavigation>();
        //public IEnumerable<INavigation> MenuTwoNavigation => _siteFooterNavContext.FooterMenuOneNavigation;
    }
}