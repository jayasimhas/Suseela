using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Site
{
	public interface ISiteRootsContext
	{
		IEnumerable<ISite_Root> SiteRoots { get; }
	}
}