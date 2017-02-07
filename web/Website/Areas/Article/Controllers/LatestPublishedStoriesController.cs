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
            MediaType = !string.IsNullOrEmpty(Parameters.Media_Type.ToString()) ? Parameters.Media_Type : Guid.Empty;
            ContentType = !string.IsNullOrEmpty(Parameters.Content_Type.ToString()) ? Parameters.Content_Type : Guid.Empty;
            ItemsToDisplay = !string.IsNullOrEmpty(Parameters.Max_Stories_to_Display.ToString()) ? Parameters.Max_Stories_to_Display : 4;
            PublicationName = rootContext.Item.Publication_Name;

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

            if (Topics != null) filter.TaxonomyIds.AddRange(Topics);
            if (PublicationName != null) filter.PublicationNames.Add(PublicationName);
            if (Authors != null) filter.AuthorGuids.AddRange(Authors);
            if (!string.IsNullOrEmpty(MediaType.ToString()) && MediaType != Guid.Empty) filter.MediaTypeTaxonomyId = MediaType;
            if (!string.IsNullOrEmpty(ContentType.ToString()) && ContentType != Guid.Empty) filter.ContentTypeTaxonomyId = ContentType;

            latest.MaxStoriesToDisplay = !string.IsNullOrEmpty(Parameters.Max_Stories_to_Display.ToString()) ? Parameters.Max_Stories_to_Display : 4;

            if (latest.MaxStoriesToDisplay < 4 && latest.MaxStoriesToDisplay != 0)
            {
                filter.PageSize = latest.MaxStoriesToDisplay;
            }
            else
            {
                filter.PageSize = 4;
            }
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
            latest.LoadMoreText = TextTranslator.Translate("Load.More.Text");

            return View("~/Areas/Article/Views/LatestPublishedStories/LatestPublishedStories.cshtml", latest);
        }
        /// <summary>
        /// Method to get latest stories from second call onwards
        /// </summary>
        /// <param name="subjectIds">Taxonomies</param>
        /// <param name="publicationName">Publication name</param>
        /// <param name="authorGuids">Author Guids</param>
        /// <param name="itemsToDisplay">Number of stories displayed</param>
        /// <param name="MaxStoriesToDisplay">Max number of stroies to display</param>
        /// <param name="mediaType">Media Type</param>
        /// <param name="contentType">Content type</param>
        /// <returns></returns>
        [HttpPost]
        public string GetLatestNews(string subjectIds, string publicationName, string authorGuids, int itemsToDisplay, int MaxStoriesToDisplay, Guid mediaType, Guid contentType)
        {
            IList<Guid> subjectGuids = new List<Guid>();
            if (!string.IsNullOrEmpty(subjectIds))
            {
                IList<string> subjectsArray = new List<string>();
                if (subjectIds.Contains(","))
                {
                    subjectsArray = subjectIds.Split(',').ToList();
                    foreach (var subject in subjectsArray)
                    {
                        subjectGuids.Add(new Guid(subject));
                    }
                }
                else
                {
                    subjectGuids.Add(new Guid(subjectIds));
                }
            }

            IList<string> authorIds = new List<string>();
            if (!string.IsNullOrEmpty(authorGuids))
            {

                if (subjectIds.Contains(","))
                {
                    authorIds = authorGuids.Split(',').ToList();
                }
                else
                {
                    authorIds.Add(authorGuids);
                }
            }
            var filter = ArticleSearch.CreateFilter();
            filter.Page = itemsToDisplay;

            if (subjectGuids != null) filter.TaxonomyIds.AddRange(subjectGuids);
            if (publicationName != null) filter.PublicationNames.Add(publicationName);
            if (authorIds != null && authorIds.Any()) filter.AuthorGuids.AddRange(authorIds);
            if (!string.IsNullOrEmpty(mediaType.ToString()) && mediaType != Guid.Empty) filter.MediaTypeTaxonomyId = mediaType;
            if (!string.IsNullOrEmpty(contentType.ToString()) && contentType != Guid.Empty) filter.ContentTypeTaxonomyId = contentType;

            if ((itemsToDisplay - 1) * 4 + 4 > MaxStoriesToDisplay && MaxStoriesToDisplay != 0)
            {
                filter.PageSize = MaxStoriesToDisplay - (itemsToDisplay - 1) * 4;
            }
            else
            {
                filter.PageSize = 4;
            }
            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));

            List<object> objs = new List<object>();
            foreach (var article in articles)
            {
                var dateDiff = DateTime.Now - article.ListableDate;
                string datelbl = "";
                if (dateDiff.TotalMinutes < 60)
                {
                    datelbl = dateDiff.Minutes + " min ago";
                }
                else if (dateDiff.TotalHours < 24)
                {
                    datelbl = dateDiff.Hours + " hours ago";
                }
                else
                {
                    datelbl = article.ListableDate.ToString("dd MMM yyyy");
                }
                var obj = new
                {
                    title = article.ListableTitle,
                    url = article.LinkableUrl,
                    pubDate = datelbl
                };
                objs.Add(obj);

            }
            return JsonConvert.SerializeObject(objs, Formatting.Indented);
        }

        public IEnumerable<IListableViewModel> News { get; set; }
        public ILatest_Published_Stories_Options Parameters { get; set; }
        public IList<string> Authors { get; set; }
        public IEnumerable<Guid> Topics { get; set; }
        public int ItemsToDisplay { get; set; }
        public string PublicationName { get; set; }
        public Guid ContentType { get; set; }
        public Guid MediaType { get; set; }
        public string RemoveSpecialCharactersFromGuid(string guid)
        {
            return guid.Replace("-", "").Replace("{", "").Replace("}", "").ToLower();
        }
    }
}
