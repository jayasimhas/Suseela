﻿using System.Collections.Generic;
using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
	public interface IArticleAuthorResults
	{
		[IndexField("authors_sm")]
		IList<string> AuthorGuid { get; set; }

		[IndexField("facetauthornames_sm")]
		IList<string> AuthorFullNames { get; set; }
	}
}