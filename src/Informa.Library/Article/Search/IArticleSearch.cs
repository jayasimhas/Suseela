using System;
using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearch
	{
		IArticleSearchResults Search(IArticleSearchFilter filter);
		IArticleSearchResults SearchCustomDatabase(IArticleSearchFilter filter, string database, Guid publicationGuid = default(Guid));
        IArticleSearchResults FreeWithRegistrationArticles(string database);
        IArticleSearchFilter CreateFilter();
		long GetNextArticleNumber(Guid publicationGuid);
        string GetPublicationPrefix(string publicationGuid);
        string GetArticleTaxonomies(Guid id, Guid taxonomyParent);
		IArticleSearchResults GetLegacyArticleUrl(string path);
        IArticleSearchResults PersonalizedSearch(IArticleSearchFilter filter, ID topicOrChannelId);

    }
}
