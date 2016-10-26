namespace Informa.Web.Controllers.Search
{
    using Informa.Library.Article.Search;
    using Informa.Web.ViewModels.Articles;
    using Library.Globalization;
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Results;
    public class ArticleSearchController : ApiController
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly ITextTranslator TextTranslator;
        public ArticleSearchController(IArticleSearch articleSearch, IArticleListItemModelFactory articleListableFactory,
            ITextTranslator textTranslator)
        {
            ArticleSearch = articleSearch;
            ArticleListableFactory = articleListableFactory;
            TextTranslator = textTranslator;
        }
        public object GetArticles(string pid, int pno, int psize)
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = pno;
            filter.PageSize = psize;
            filter.TaxonomyIds.Add(new Guid(pid));
            var results = ArticleSearch.PersonalizedSearch(filter);
            if (results != null && results.Articles != null && results.Articles.Count() > psize - 1)
            {
                var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));

                var loadMore = new LoadMore
                {
                    DisplayLoadMore = (results.TotalResults - (pno * psize)) >= psize,
                    LoadMoreLinkText = TextTranslator.Translate("Article.LoadMoreFrom"),
                    LoadMoreLinkUrl = $"/api/articlesearch/?pid={pid}&pno={pno + 1}&psize={psize}",
                    LatestFromText = TextTranslator.Translate("Article.LatestFrom")
                };
                return new { Articles = articles, LoadMore = loadMore };
            }

            return new { Articles = "No articles found" };
        }
    }
}
