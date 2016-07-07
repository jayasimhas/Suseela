﻿using Informa.Library.ContentCuration.Search.Filter;
using Informa.Library.Search.Filter;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearchFilter : IManuallyCuratedContentFilter, ITaxonomySearchFilter, IArticleNumbersFilter, IArticleEScenicIDFilter, IReferencedArticleFilter, IArticlePublicationFilter, IArticleAuthorFilter
    {
		int Page { get; set; }
		int PageSize { get; set; }
	}
}
