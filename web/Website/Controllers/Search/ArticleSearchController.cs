namespace Informa.Web.Controllers.Search
{
    using Informa.Library.Article.Search;
    using Informa.Web.ViewModels.Articles;
    using Library.Globalization;
    using Library.Utilities.Extensions;
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

        /// <summary>
        /// Gets articles based on user preferences
        /// </summary>
        /// <param name="articleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetArticles([FromBody] ArticleSearchRequest articleRequest)
        {
            if(articleRequest == null || articleRequest.TaxonomyIds == null || articleRequest.TaxonomyIds.Count < 1)
                return new { Articles = "No articles found" };
            var filter = ArticleSearch.CreateFilter();
            filter.Page = articleRequest.PageNo;
            filter.PageSize = articleRequest.PageSize;
            filter.TaxonomyIds.AddRange(articleRequest.TaxonomyIds.Select(x => new Guid(x)));
            var results = ArticleSearch.PersonalizedSearch(filter);
            if (results != null && results.Articles != null && results.Articles.Count() > articleRequest.PageSize - 1)
            {
                var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));
                if(articles == null || articles.Count() < articleRequest.PageSize)
                    return new { Articles = "No articles found" };

                var loadMore = new LoadMore
                {
                    DisplayLoadMore = (results.TotalResults - (articleRequest.PageNo * articleRequest.PageSize)) >= articleRequest.PageSize,
                    LoadMoreLinkText = TextTranslator.Translate("Article.LoadMoreFrom"),
                    LoadMoreLinkUrl = "/api/articlesearch/",
                    LatestFromText = TextTranslator.Translate("Article.LatestFrom"),
                    PageNo = articleRequest.PageNo + 1,
                    PageSize = articleRequest.PageSize,
                    TaxonomyIds = articleRequest.TaxonomyIds
                };
                return new { Articles = articles, LoadMore = loadMore };
            }

            return new { Articles = "No articles found" };
        }
    }
}
