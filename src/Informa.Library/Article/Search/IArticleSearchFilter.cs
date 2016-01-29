using Informa.Library.ContentCuration.Search.Filter;

namespace Informa.Library.Article.Search
{
	public interface IArticleSearchFilter : IManuallyCuratedContentFilter
	{
		int Page { get; set; }
		int PageSize { get; set; }
	}
}
