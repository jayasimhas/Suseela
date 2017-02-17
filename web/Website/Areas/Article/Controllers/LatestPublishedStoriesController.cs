using Informa.Library.Article.Search;
using Informa.Library.Authors;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Informa.Web.Areas.Article.Models.Article.LatestPublishedStories;
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

            Authors = new List<string>();
            Parameters = renderingParametersService.GetCurrentRenderingParameters<ILatest_Published_Stories_Options>();
            if (Parameters == null) return;
            Authors = Parameters.Authors?.Select(p => RemoveSpecialCharactersFromGuid(p._Id.ToString())).ToArray();
            Topics = Parameters.Subjects.Select(s => s._Id).ToArray();
            MediaType = Parameters?.Media_Type;
            ContentType = Parameters?.Content_Type;
            ItemsToDisplay = Parameters.Max_Stories_to_Display != 0 ? Parameters.Max_Stories_to_Display : 25;
            PublicationName = rootContext.Item.Publication_Name;
            IsDisplayDate = Parameters?.Display_Published_Date ?? false;
            ComponentHeight = Parameters?.Component_Height != 0 ? Parameters.Component_Height : 625;
        }
        /// <summary>
        /// Get latest published strories in first call
        /// </summary>
        /// <returns>4 latest stories</returns>
        [HttpGet]
        public ActionResult GetLatestNews()
        {
            LatestPublishedStory latest = new LatestPublishedStory();
            var filter = ArticleSearch.CreateFilter();
            filter.Page = 1;
            filter.PageSize = ItemsToDisplay;

            if (Topics != null) filter.TaxonomyIds.AddRange(Topics);
            if (PublicationName != null) filter.PublicationNames.Add(PublicationName);
            if (Authors != null) filter.AuthorGuids.AddRange(Authors);
            if (MediaType != null) filter.MediaTypeTaxonomyIds.AddRange(MediaType);
            if (ContentType != null) filter.ContentTypeTaxonomyIds.AddRange(ContentType);

            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));

            latest.News = articles;
            latest.PublicationName = PublicationName;
            latest.Topics = Topics;
            latest.Authors = Authors;
            latest.ContentType = ContentType;
            latest.MediaType = MediaType;
            latest.IsDisplayDate = IsDisplayDate;
            latest.ComponentHeight = ComponentHeight;
            latest.LoadMoreText = TextTranslator.Translate("Load.More.Text");
            latest.LatestStoriesComponentTitle = TextTranslator.Translate("Latest.Published.Stories.Component.Title");
            return View("~/Areas/Article/Views/LatestPublishedStories/LatestPublishedStories.cshtml", latest);
        }

        public IEnumerable<IListableViewModel> News { get; set; }
        public ILatest_Published_Stories_Options Parameters { get; set; }
        public IList<string> Authors { get; set; }
        public IEnumerable<Guid> Topics { get; set; }
        public int ItemsToDisplay { get; set; }
        public string PublicationName { get; set; }
        public IList<Guid> ContentType { get; set; }
        public IList<Guid> MediaType { get; set; }
        public bool IsDisplayDate { get; set; }
        public int ComponentHeight { get; set; }
        public string RemoveSpecialCharactersFromGuid(string guid)
        {
            return guid.Replace("-", "").Replace("{", "").Replace("}", "").ToLower();
        }
    }
}
