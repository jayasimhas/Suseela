using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearchResults
	{
		IEnumerable<IArticle> Articles { get; }
	}
}
