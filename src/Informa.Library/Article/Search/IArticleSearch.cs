using System;
using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearch
	{
		IArticleSearchResults Search(IArticleSearchFilter filter);
		IArticleSearchResults SearchCustomDatabase (IArticleSearchFilter filter, string database);
        IEnumerable<IArticle> NewsSitemapSearch(string path);
        IArticleSearchFilter CreateFilter();
		long GetNextArticleNumber(Guid publicationGuid);
	}
}
