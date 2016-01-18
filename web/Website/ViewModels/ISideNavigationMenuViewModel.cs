using Informa.Library.Navigation;
using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
	public interface ISideNavigationMenuViewModel
	{
		IEnumerable<INavigation> Navigation { get; }

		string MenuText { get; }

		string MenuButtonText { get; }
	}
}
