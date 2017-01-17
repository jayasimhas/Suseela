using System;
using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearch
	{
		IArticleSearchResults Search(IArticleSearchFilter filter);
        IArticleSearchResults SearchArticleByAuthorName(IArticleSearchFilter filter);        
        IArticleSearchResults SearchCustomDatabase(IArticleSearchFilter filter, string database);
        IArticleSearchResults FreeWithRegistrationArticles(string database);
        IArticleSearchFilter CreateFilter();
		long GetNextArticleNumber(Guid publicationGuid);
		string GetArticleTaxonomies(Guid id, Guid taxonomyParent);
		IArticleSearchResults GetLegacyArticleUrl(string path);
	}
}
