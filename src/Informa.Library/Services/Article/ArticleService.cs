using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;
using Informa.Library.Search.Utilities;
using Informa.Library.Site;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.System.Dictionary;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Jabberwocky.Core.Caching;
using Sitecore.Resources.Media;
using Informa.Library.Services.Global;
using Sitecore.Data.Items;
using Sitecore.Web;
using System.Web;

namespace Informa.Library.Services.Article
{

    [AutowireService(LifetimeScope.SingleInstance)]
    public class ArticleService : IArticleService
    {
        protected readonly ICacheProvider CacheProvider;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISiteRootsContext SiteRootsContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IDCDTokenMatchers DCDTokenMatchers;

        public ArticleService(
            ICacheProvider cacheProvider,
            ITextTranslator textTranslator,
			ISiteRootsContext siteRootsContext,
            IGlobalSitecoreService globalService,
            IDCDTokenMatchers dcdTokenMatchers)
        {
            CacheProvider = cacheProvider;
            TextTranslator = textTranslator;
            SiteRootsContext = siteRootsContext;
            GlobalService = globalService;
            DCDTokenMatchers = dcdTokenMatchers;
        }

        private string CreateCacheKey(string suffix)
        {
            return $"{nameof(ArticleService)}-{suffix}";
        }

        public IEnumerable<string> GetLegacyPublicationNames(IArticle article)
        {
            string cacheKey = CreateCacheKey($"PublicationNames-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildLegacyPublicationNames(article));
        }

        private IEnumerable<string> BuildLegacyPublicationNames(IArticle article)
        {

            List<string> l = new List<string>();
            foreach (IGlassBase g in article.Legacy_Publications)
            {
                if (g == null)
                    continue;

                l.Add(((ITaxonomy_Item)g).Item_Name);
            }

            return l;
        }

        public IEnumerable<ILinkable> GetLinkableTaxonomies(IArticle article)
        {
            string cacheKey = CreateCacheKey($"LinkableTaxonomies-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildLinkableTaxonomies(article));
        }

        private IEnumerable<ILinkable> BuildLinkableTaxonomies(IArticle article)
        {

            var taxItems = article.Taxonomies?
                .Select(x => new LinkableModel
                {
                    LinkableText = x.Item_Name,
                    LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x)
                }).ToList();

            return taxItems;
        }

        public MediaTypeIconData GetMediaTypeIconData(IArticle article)
        {
            if(article?.Media_Type == null) { return null; }

            var mediaType = article.Media_Type.Item_Name == "Data"
                ? "chart"
                : article.Media_Type.Item_Name?.ToLower() ?? string.Empty;

            var tooltipText = article.Media_Type.Tooltip;

            return new MediaTypeIconData() {MediaType = mediaType, Tooltip = tooltipText};
        }

        public string GetArticleSummary(IArticle article)
        {
            string cacheKey = CreateCacheKey($"Summary-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildTokenizedArticleText(article.Summary));
        }

        public string GetArticleBody(IArticle article)
        {
            string cacheKey = CreateCacheKey($"Body-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildTokenizedArticleText(article.Body));
        }

        private string BuildTokenizedArticleText(string text)
        {
            return DCDTokenMatchers.ProcessDCDTokens(text);
        }

        public string GetLegacyPublicationText(IArticle article)
        {
            var legacyText = TextTranslator.Translate("Article.LegacyPublications");
            var legacyPublicationsText = GetLegacyPublicationNames(article).JoinWithFinal(", ", "&");

            return legacyText.Replace("{Legacy Publications}", legacyPublicationsText);
        }

        public IEnumerable<IFile> GetSupportingDocuments(IArticle article)
        {
            string cacheKey = CreateCacheKey($"SupportingDocs-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildSupportingDocuments(article));
        }

        private IEnumerable<IFile> BuildSupportingDocuments(IArticle article)
        {
            return article.Supporting_Documents.OfType<IFile>();//.Select(a => (IFile) a).ToList();
        }

        public string GetArticlePublicationName(IArticle article)
        {
            string cacheKey = CreateCacheKey($"ArticlePublicationCode-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildArticlePublicationName(article));
        }

        private string BuildArticlePublicationName(IArticle article)
        {
            var siteRoot = SiteRootsContext
                .SiteRoots
                .FirstOrDefault(i => article._Path.StartsWith(i._Path));

            return (siteRoot != null)
                ? siteRoot.Publication_Name
                : string.Empty;
        }

        public string GetDownloadUrl(IArticle article) {
            string cacheKey = CreateCacheKey($"GetDownloadLink-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildDownloadUrl(article));
        }
        protected string BuildDownloadUrl(IArticle article) {

            if (article.Word_Document == null)
                return string.Empty;

            Item wordDoc =  GlobalService.GetItem<Item>(article.Word_Document.TargetId);
            if (wordDoc == null) 
                return string.Empty;
            
            string url = MediaManager.GetMediaUrl(wordDoc)
                .Replace("/-/", "/~/")
                .Replace("-", " ");
            return url;
        }

        public string GetPreviewUrl(IArticle article) {
            string cacheKey = CreateCacheKey($"GetPreviewLink-{article._Id}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildPreviewUrl(article));
        }

        protected string BuildPreviewUrl(IArticle article) {
            string previewUrl = HttpContext.Current.Request.Url.Scheme + "://" + WebUtil.GetHostName() + "/?sc_itemid={" + article._Id + "}&sc_mode=preview&sc_lang=en";
            string fullLink = $"/VWB/Util/LoginRedirectToPreview.aspx?redirect={HttpUtility.UrlEncode(previewUrl)}";

            return fullLink;
        }
    }
}
