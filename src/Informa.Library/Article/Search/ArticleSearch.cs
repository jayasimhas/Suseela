using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Article.Search
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticleSearch : IArticleSearch
	{
		public IArticleSearchFilter CreateFilter()
		{
			return new ArticleSearchFilter();
		}

		public IArticleSearchResults Search(IArticleSearchFilter filter)
		{
			return null;
		}
	}
}
