using Informa.Library.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Site
{
	public interface ISiteMainNavigationContext
	{
		IEnumerable<INavigation> Navigation { get; }
	}
}
