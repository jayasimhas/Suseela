using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public class ArticleSearchResults : IArticleSearchResults
	{
		public IEnumerable<IArticle> Articles { get; set; }
	}
}
