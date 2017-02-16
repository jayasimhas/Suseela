using System;
using System.Collections.Generic;

namespace Informa.Library.Search.Filter
{
	public interface ITaxonomySearchFilter
	{
		IList<Guid> TaxonomyIds { get; }
        IList<Guid> ContentTypeTaxonomyIds { get; set; }
        IList<Guid> MediaTypeTaxonomyIds { get; set; }
    }
}
