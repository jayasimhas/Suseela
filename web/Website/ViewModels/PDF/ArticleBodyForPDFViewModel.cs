using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Globalization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;
using Informa.Library.Services.Article;
using Informa.Library.Site;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.Extensions;
using Informa.Models.FactoryInterface;
using Informa.Web.ViewModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Article.Search;
using Informa.Library.Services.Global;
using Sitecore.Mvc.Presentation;
using System.Text.RegularExpressions;

namespace Informa.Web.ViewModels.PDF
{
    public class ArticleBodyForPDFViewModel : ArticleEntitledViewModel
    {
        public readonly ICallToActionViewModel CallToActionViewModel;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleService ArticleService;
        protected readonly ISiteRootContext SiteRootContext;
        private readonly Lazy<string> _lazyBody;
        public IArticleTagsViewModel ArticleTagsViewModel;
        protected readonly IArticleSearch Searcher;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly IGlobalSitecoreService GlobalService;

        public ArticleBodyForPDFViewModel(IArticle model,
                        IIsEntitledProducItemContext entitledProductContext,
                        ITextTranslator textTranslator,
                        ICallToActionViewModel callToActionViewModel,
                        IArticleService articleService,
                        IAuthenticatedUserContext authenticatedUserContext,
                        ISiteRootContext siteRootContext,
                        ISitecoreUserContext sitecoreUserContext,
                        IArticleTagsViewModel articleTagsViewModel,
                        IArticleListItemModelFactory articleListableFactory,
                        IArticleSearch searcher,
                        IGlobalSitecoreService globalService)
                        : base(entitledProductContext, authenticatedUserContext, sitecoreUserContext)
        {
            TextTranslator = textTranslator;
            CallToActionViewModel = callToActionViewModel;
            ArticleService = articleService;
            SiteRootContext = siteRootContext;
            ArticleTagsViewModel = articleTagsViewModel;
            ArticleListableFactory = articleListableFactory;
            Searcher = searcher;
            GlobalService = globalService;
            RelatedArticles = GetRelatedArticles(model);
            _lazyBody = new Lazy<string>(() => IsFree || (IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated) || IsEntitled() ? ArticleService.GetArticleBody(model) : "");
        }

        /// <summary>
        /// Article Title
        /// </summary>
        public string Title => GlassModel.Title;
        /// <summary>
        /// Article SubTitle
        /// </summary>
        public string Sub_Title => GlassModel.Sub_Title;

        private string _summary;
        /// <summary>
        /// Article Summary
        /// </summary>
        public string Summary=>
        _summary ?? (_summary = ArticleService.GetArticleSummary(GlassModel));
           

        private IEnumerable<IPersonModel> _authors;
        /// <summary>
        /// List of Authors
        /// </summary>
        public IEnumerable<IPersonModel> Authors
                => _authors ?? (_authors = GlassModel.Authors.Select(x => new PersonModel(x)));

        private DateTime? _date;
        public DateTime Date
        {
            get
            {
                if (!_date.HasValue)
                {
                    _date = GlassModel.GetDate();
                }
                return _date.Value;
            }
        }
        /// <summary>
        /// Article Category
        /// </summary>
        public string Category => GlassModel.Article_Category;
        /// <summary>
        /// Article Body Content
        /// </summary>
        public string Body => GetPDFBody();

        private string GetPDFBody()
        {
            string body = string.Empty;
            body= _lazyBody.Value.Contains("<table") ? _lazyBody.Value.Replace("<table", "<table id=\"tableFromArticle\"") : _lazyBody.Value;
            body = body.StartsWith("<div")? body.Replace("<div", "<p") : body;
            body = body.EndsWith("</div>") ? body.Replace("</div>", "<p>") : body;
            return body;
        }
        public string ContentType => GlassModel.Content_Type?.Item_Name;
        public MediaTypeIconData MediaTypeIconData => ArticleService.GetMediaTypeIconData(GlassModel);
        public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
        public string FeaturedImageSource => TextTranslator.Translate("Article.FeaturedImageSource");
        public string ExecutiveSummary => TextTranslator.Translate("SharedContent.ExecutiveSummary");
        public bool IsActiveLegacyBrand => SiteRootContext.Item.Legacy_Brand_Active;
        public List<string> LagacyBrandUrl => ArticleService.GetLegacyPublicationNames(GlassModel, SiteRootContext.Item.Legacy_Brand_Active).ToList<string>();
        /// <summary>
        /// Method to Related Articles
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        private IEnumerable<IListable> GetRelatedArticles(IArticle article)
        {
            var relatedArticles = article.Related_Articles.Concat(article.Referenced_Articles).Take(10).ToList();

            if (relatedArticles.Count < 10)
            {
                var filter = Searcher.CreateFilter();
                filter.ReferencedArticle = article._Id;
                filter.PageSize = 10 - relatedArticles.Count;

                var results = Searcher.Search(filter);
                relatedArticles.AddRange(results.Articles);
            }
            return relatedArticles.Where(r => r != null).Select(x => ArticleListableFactory.Create(GlobalService.GetItem<IArticle>(x._Id))).Cast<IListable>().OrderByDescending(x => x.ListableDate);
        }

        public IEnumerable<IListable> RelatedArticles { get; set; }

        /// <summary>
        /// Article Landing Page URL
        /// </summary>
        public string ArticleLandingPageUrl => Sitecore.Links.LinkManager.GetItemUrl(RenderingContext.Current.Rendering.Item);
    }
}