using Informa.Library.Article.Search;
using Informa.Library.Authors;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Web.ViewModels;
using Informa.Web.ViewModels.Articles;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Article.Controllers
{
    public class LatestPublishedStoriesController : Controller
    {
        protected readonly IArticleSearch ArticleSearch;
        public readonly ITextTranslator TextTranslator;
        protected readonly IAuthorService AuthorService;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        public LatestPublishedStoriesController(IArticleSearch articleSearch,
            ITextTranslator textTranslator,
            IAuthorService authorService,
            IRenderingContextService renderingParametersService,
            ISiteRootContext rootContext,
            IArticleListItemModelFactory articleListableFactory)
        {
            ArticleSearch = articleSearch;
            TextTranslator = textTranslator;
            AuthorService = authorService;
            ArticleListableFactory = articleListableFactory;

            Parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_Published_Stories_Options>();
            if (Parameters == null) return;
            Authors = Parameters.Authors?.Select(p => RemoveSpecialCharactersFromGuid(p._Id.ToString())).ToArray();
            Topics = Parameters.Subjects.Select(s => s._Id).ToArray();
            ItemsToDisplay = !string.IsNullOrEmpty(Parameters.Max_Stories_to_Display.ToString()) ? Parameters.Max_Stories_to_Display : 4;
            PublicationName = rootContext.Item.Publication_Name;           

        }
        [HttpGet]
        public ActionResult GetLatestNews()
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = 1;
            filter.PageSize = 4;

            if (Topics != null) filter.TaxonomyIds.AddRange(Topics);
            if (PublicationName != null) filter.PublicationNames.Add(PublicationName);
            if (Authors != null) filter.AuthorGuids.AddRange(Authors);

            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));            
            return View("~/Areas/Article/Views/LatestPublishedStories/LatestPublishedStories.cshtml", articles);
        }
        [HttpPost]
        public string GetLatestNews(IEnumerable<Guid> subjectIds, string publicationName, IList<string> authorGuids, int itemsToDisplay)
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = 1;
            filter.PageSize = 8;

            if (Topics != null) filter.TaxonomyIds.AddRange(Topics);
            if (PublicationName != null) filter.PublicationNames.Add(PublicationName);
            if (Authors != null) filter.AuthorGuids.AddRange(Authors);

            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));
            List<object> strs = new List<object>();
            foreach (var article in articles)
            {
                var obj = new { img = article.ListableImage, title = article.ListableTitle, summary=article.ListableSummary };
                string jsonObj = JsonConvert.SerializeObject(obj);
                strs.Add(jsonObj);
            }
            //string jsonStr = JsonConvert.SerializeObject(objs);
            return strs.ToString();
        }

        public IEnumerable<IListableViewModel> News { get; set; }
        public ILatest_Published_Stories_Options Parameters { get; set; }
        public string LoadMore { get; set; }
        public IList<string> Authors { get; set; }
        public IEnumerable<Guid> Topics { get; set; }
        public int ItemsToDisplay { get; set; }
        public string PublicationName { get; set; }
        public string RemoveSpecialCharactersFromGuid(string guid)
        {
            return guid.Replace("-", "").Replace("{", "").Replace("}", "").ToLower();
        }
    }
}
