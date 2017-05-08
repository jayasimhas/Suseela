namespace Informa.Web.Controllers.Search
{
    using Glass.Mapper.Sc;
    using Informa.Library.Article.Search;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
    using Informa.Web.ViewModels.Articles;
    using Library.Globalization;
    using Library.Search.Utilities;
    using Library.Services.Global;
    using Library.Site;
    using Library.User.UserPreference;
    using Library.Utilities.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Results;
    public class ArticleSearchController : ApiController
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IUserPreferenceContext UserPreferences;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        public ArticleSearchController(IArticleSearch articleSearch, IArticleListItemModelFactory articleListableFactory,
            ITextTranslator textTranslator,
            ISitecoreContext sitecoreContext,
            IUserPreferenceContext userPreferences,
            ISiteRootContext siterootContext,
            IGlobalSitecoreService globalService)
        {
            ArticleSearch = articleSearch;
            ArticleListableFactory = articleListableFactory;
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
            UserPreferences = userPreferences;
            SiterootContext = siterootContext;
            GlobalService = globalService;
        }

        /// <summary>
        /// Gets articles based on user preferences
        /// </summary>
        /// <param name="articleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetArticles([FromBody] ArticleSearchRequest articleRequest)
        {
            if (articleRequest == null || articleRequest.TaxonomyIds == null || articleRequest.TaxonomyIds.Count < 1)
                return new { Articles = "No articles found" };
            var filter = ArticleSearch.CreateFilter();
            filter.Page = articleRequest.PageNo;
            filter.PageSize = articleRequest.PageSize;
            filter.TaxonomyIds.AddRange(articleRequest.TaxonomyIds.Select(x => new Guid(x)));
            var results = ArticleSearch.PersonalizedSearch(filter, null, null);
            if (results != null && results.Articles != null && results.Articles.Count() > articleRequest.PageSize - 1)
            {
                var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));
                if (articles == null || articles.Count() < articleRequest.PageSize)
                    return new { Articles = "No articles found" };

                var loadMore = new LoadMore
                {
                    DisplayLoadMore = (results.TotalResults - (articleRequest.PageNo * articleRequest.PageSize)) >= articleRequest.PageSize,
                    LoadMoreLinkText = TextTranslator.Translate("Article.LoadMoreFrom"),
                    LoadMoreLinkUrl = "/api/articlesearch/",
                    LatestFromText = TextTranslator.Translate("Channel.Prefix.Text"),
                    PageNo = articleRequest.PageNo + 1,
                    PageSize = articleRequest.PageSize,
                    TaxonomyIds = articleRequest.TaxonomyIds,
                    SeeAllText = TextTranslator.Translate("Article.LatestFrom.SeeAllLink"),
                    SeeAllLink = GetSeeAllLink(articleRequest.ChannelId),
                    CurrentlyViewingText = GetCurrentlyViewingText(articleRequest.TaxonomyIds, articleRequest.ChannelId)
                };
                return new { Articles = articles, LoadMore = loadMore };
            }

            return new { Articles = "No articles found" };
        }

        private string GetSeeAllLink(string channelId)
        {
            if (string.IsNullOrEmpty(channelId))
                return "/search#?";
            var channel = GlobalService.GetItem<IChannel_Page>(channelId);
            if (channel != null && channel.Taxonomies != null && channel.Taxonomies.Any())
            {
                return SearchTaxonomyUtil.GetSearchUrl(channel.Taxonomies.FirstOrDefault());
            }
            return "/search#?";
        }

        private string GetCurrentlyViewingText(IList<string> selectedTopicsCount, string ChannelId)
        {
            var currentlyViewingText = TextTranslator.Translate("Currently.Viewing.Out.of.Available");
            var preferredTopicsText = GetPreferredTopics(ChannelId);
            var allTopicsText = GetAllTopics(ChannelId);
            if (!string.IsNullOrEmpty(currentlyViewingText) && !string.IsNullOrEmpty(preferredTopicsText) && !string.IsNullOrEmpty(allTopicsText))
            {
                var replacements = new Dictionary<string, string>
                {
                    ["#selectedtopics#"] = preferredTopicsText,
                    ["#totaltopics#"] = allTopicsText
                };
                return currentlyViewingText.ReplacePatternCaseInsensitive(replacements);
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetPreferredTopics(string channelId)
        {
            if (!string.IsNullOrEmpty(channelId) && UserPreferences != null && UserPreferences.Preferences != null &&
                             UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count() > 0)
            {
                var channel = UserPreferences.Preferences.PreferredChannels.Where(c => c.ChannelId == channelId).FirstOrDefault();
                if (channel != null && channel.IsFollowing && UserPreferences.Preferences.IsNewUser)
                {
                    return GetAllTopics(channelId);
                }
                else
                {
                    return channel?.Topics?.Where(n => n.IsFollowing)?.Count().ToString();
                }
            }
            return string.Empty;
        }

        private string GetAllTopics(string channelId)
        {
            if (!string.IsNullOrEmpty(channelId))
            {
                var channel = SitecoreContext.GetItem<IChannel_Page>(new Guid(channelId));
                if (channel != null)
                {
                    var pageAssets = channel._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                    if (pageAssets != null)
                    {
                        var topics = pageAssets._ChildrenWithInferType.
                                OfType<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>();
                        return topics?.Count().ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}
