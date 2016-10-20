using Informa.Library.Article.Search;
using Informa.Library.Utilities.Extensions;
using Informa.Web.ViewModels.Articles;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Informa.Web.Controllers.Search
{
    public class ArticleSearchController : ApiController
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        public ArticleSearchController(IArticleSearch articleSearch, IArticleListItemModelFactory articleListableFactory)
        {
            ArticleSearch = articleSearch;
            ArticleListableFactory = articleListableFactory;
        }
        public IPersonalizedArticleSearchResults GetArticles(string url)
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = 1;
            filter.PageSize = 9;
            var topicOrChannelId = new ID("4BD861A8-09A1-4452-9B78-0E7B64ED9632");
            filter.TaxonomyIds.Add(new Guid("4BD861A8-09A1-4452-9B78-0E7B64ED9632"));
            var results = ArticleSearch.PersonalizedSearch(filter, topicOrChannelId);
            var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));

            var loadMore = new LoadMore
            {
                LoadMoreLinkText = "LoadMore",
                LoadMoreLinkUrl = "/api/articlesearch/?preferenceId=4BD861A8-09A1-4452-9B78-0E7B64ED9632&pno=2&psize=9"
            };
            return new PersonalizedArticleSearchResults { Articles = articles, LoadMore = loadMore };
            //return articles;
            
        }
    }
}
