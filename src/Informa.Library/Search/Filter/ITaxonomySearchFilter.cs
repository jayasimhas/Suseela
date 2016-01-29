using System;
using System.Collections.Generic;

namespace Informa.Library.Search.Filter
{
	public interface ITaxonomySearchFilter
	{
		IList<Guid> TaxonomyIds { get; }
	}
}
