using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public interface IArticleAuthorFilter
	{
		IList<string> AuthorGuids { get; set; }
		IList<string> AuthorFullNames { get; set; } 
	}
}