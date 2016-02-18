using System;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearch
	{
		IArticleSearchResults Search(IArticleSearchFilter filter);
		IArticleSearchFilter CreateFilter();
		int GetNextArticleNumber(Guid publicationGuid);
	}
}
