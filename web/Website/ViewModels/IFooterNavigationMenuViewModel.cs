
using Informa.Library.Navigation;
using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
    public interface IFooterNavigationMenuViewModel
    {
        IEnumerable<INavigation> MenuOneNavigation { get; }

        IEnumerable<INavigation> MenuTwoNavigation { get; }
    }
}