using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
	public interface IArticlePublicationFilter
	{
		IList<string> PublicationNames { get; set; } 
	}
}