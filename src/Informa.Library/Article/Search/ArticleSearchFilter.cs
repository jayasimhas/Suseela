namespace Informa.Library.Article.Search
{
	public class ArticleSearchFilter : IArticleSearchFilter
	{
		public int Page { get; set; }
		public int ResultsPerPage { get; set; }
	}
}
