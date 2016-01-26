using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;

namespace Informa.Library.Search.Models
{
	public interface IManuallyCuratedContent
	{
		[IndexField("ManuallyCuratedContent")]
		IEnumerable<Guid> ManuallyCuratedItems { get; }
	}
}
