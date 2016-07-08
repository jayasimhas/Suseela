using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public interface IArticleAuthorFilter
	{
		IList<string> AuthorNames { get; set; } 
	}
}