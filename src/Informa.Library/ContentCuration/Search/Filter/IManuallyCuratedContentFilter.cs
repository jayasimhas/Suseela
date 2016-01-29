using System;
using System.Collections.Generic;

namespace Informa.Library.ContentCuration.Search.Filter
{
	public interface IManuallyCuratedContentFilter
	{
		IList<Guid> ExcludeManuallyCuratedItems { get; }
	}
}
