﻿using System;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearch
	{
		IArticleSearchResults Search(IArticleSearchFilter filter);
		IArticleSearchResults SearchCustomDatabase (IArticleSearchFilter filter, string database);
		IArticleSearchFilter CreateFilter();
		long GetNextArticleNumber(Guid publicationGuid);
	}
}
