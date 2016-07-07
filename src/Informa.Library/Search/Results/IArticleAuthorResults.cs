using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
	public interface IArticleAuthorResults
	{
		[IndexField("authors_sm")]
		string AuthorGuid { get; set; }
	}
}