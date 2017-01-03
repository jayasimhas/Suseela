using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Informa.Library.Utilities.References
{
	public interface ITaxonomyFolders
	{
		IEnumerable<Item> TaxonomyFolders { get; }
	}
}
