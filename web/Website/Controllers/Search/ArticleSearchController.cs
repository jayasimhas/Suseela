namespace Informa.Web.Controllers.Search
{
    using Informa.Library.Article.Search;
    using Informa.Web.ViewModels.Articles;
    using Library.Globalization;
    using System;
    using System.Linq;
    using System.Web.Http;
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
        public IPersonalizedArticleSearchResults GetArticles(string pid, int pno, int psize)
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = pno;
            filter.PageSize = psize;
            //string topicOrChannelId = "4BD861A8-09A1-4452-9B78-0E7B64ED9632";
            filter.TaxonomyIds.Add(new Guid(pid));
            var results = ArticleSearch.PersonalizedSearch(filter);
            var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));

            var loadMore = new LoadMore
            {
                DisplayLoadMore = (results.TotalResults - (pno * psize)) >= psize,
                LoadMoreLinkText = TextTranslator.Translate("Article.LatestFrom"),
                LoadMoreLinkUrl = $"/api/articlesearch/?pid={pid}&pno={pno+1}&psize={psize}"
            };
            return new PersonalizedArticleSearchResults { PersonalizedArticles = articles, LoadMore = loadMore, TotalResults = results.TotalResults };
            //return articles;
            
        }
    }
}
