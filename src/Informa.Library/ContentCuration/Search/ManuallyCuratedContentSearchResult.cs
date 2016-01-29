using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;

namespace Informa.Library.ContentCuration.Search
{
	public class ManuallyCuratedContentSearchResult : SearchResultItem
	{
		[IndexField("ManuallyCuratedContent")]
		public List<Guid> ManuallyCuratedContent { get; set; }
	}
}
