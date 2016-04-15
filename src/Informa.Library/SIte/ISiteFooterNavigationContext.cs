using Informa.Library.Navigation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Site
{
    public interface ISiteFooterNavigationContext
    {
        INavigation_Root NavigationMenuOneRoot { get; }

        INavigation_Root NavigationMenuTwoRoot { get; }
        //IEnumerable<INavigation> FooterMenuOneNavigation { get; }

        //IEnumerable<INavigation> FooterMenuTwoNavigation { get; }
    }
}
