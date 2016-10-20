namespace Informa.Web.Controllers.Search
{
    using Informa.Library.Article.Search;
    using Informa.Web.ViewModels.Articles;
    using System;
    using System.Linq;
    using System.Web.Http;
    public class ArticleSearchController : ApiController
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        public ArticleSearchController(IArticleSearch articleSearch, IArticleListItemModelFactory articleListableFactory)
        {
            ArticleSearch = articleSearch;
            ArticleListableFactory = articleListableFactory;
        }
        public IPersonalizedArticleSearchResults GetArticles(string pid, string pno, string psize)
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = 1;
            filter.PageSize = 3;
            filter.TaxonomyIds.Add(new Guid("4BD861A8-09A1-4452-9B78-0E7B64ED9632"));
            var results = ArticleSearch.PersonalizedSearch(filter);
            var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));

            var loadMore = new LoadMore
            {
                LoadMoreLinkText = "LoadMore",
                LoadMoreLinkUrl = "/api/articlesearch/?preferenceId=4BD861A8-09A1-4452-9B78-0E7B64ED9632&pno=2&psize=9"
            };
            return new PersonalizedArticleSearchResults { PersonalizedArticles = articles, LoadMore = loadMore, TotalResults = results.TotalResults };
            //return articles;
            
        }
    }
}
