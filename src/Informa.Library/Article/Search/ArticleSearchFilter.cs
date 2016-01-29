using System;
using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public class ArticleSearchFilter : IArticleSearchFilter
	{
		public IList<Guid> ExcludeManuallyCuratedItems { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
	}
}
