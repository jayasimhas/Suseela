namespace Informa.Library.Article.Search
{
	public interface IArticleSearchFilter
	{
		int Page { get; set; }
		int ResultsPerPage { get; set; }
	}
}
