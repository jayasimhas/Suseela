using System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation;
using System.Collections.Generic;

namespace Informa.Library.Navigation
{
	public interface IItemNavigationTreeFactory
	{
        IEnumerable<INavigation> Create(Guid navigationRootId);
        IEnumerable<INavigation> Create(INavigation_Root navigationRootItem);
	}
}
