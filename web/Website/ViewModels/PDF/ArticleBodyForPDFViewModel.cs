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
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Sitecore.Data.Items;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using log4net;

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
        private readonly ILog _logger;
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
                        IGlobalSitecoreService globalService,
                        ILog logger)
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
            _logger = logger;
            _lazyBody = new Lazy<string>(() => IsFree || (IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated) || IsEntitled(model) ? ArticleService.GetArticleBody(model) : "");
            //_lazyBody = new Lazy<string>(() => ArticleService.GetArticleBody(model));
        }
        Guid curItemID => GlassModel._Id;
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
        public string Summary =>
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
            body = _lazyBody.Value.Contains("<table") ? _lazyBody.Value.Replace("<table", "<table cellpadding=\"10\" cellspacing=\"0\" id=\"tableFromArticle\"") : _lazyBody.Value;
            body = body.StartsWith("<div") ? body.Replace("<div", "<p") : body;
            body = body.EndsWith("</div>") ? body.Replace("</div>", "<p>") : body;
            return body;
        }
        public string MoreInText => TextTranslator.Translate("Article.ArticlePackageMoreInText");
        public bool IsArticlePage => GlassModel is IArticle;
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
        public IArticle ArticleItem => GlassModel;


        public IEnumerable<IArticle_Package> GetPackageReferrersbyLoop()
        {
            try
            {
                Item curItem = GlobalService.GetItem<Item>(curItemID);
                if (curItem == null) return null;
                //IEnumerable<IArticle_Package> articlePackage;
                //Get the packages root item
                var verticalGlobal = GlobalService.GetVerticalRootAncestor(GlassModel._Id)?._ChildrenWithInferType.OfType<IEnvironment_Global_Root>().FirstOrDefault();
                var packageFolder = verticalGlobal._ChildrenWithInferType.OfType<IPackage_Folder>().FirstOrDefault();
                if (packageFolder == null) return null;
                var articlePackages = packageFolder._ChildrenWithInferType.OfType<IArticle_Package>()?.Where(p => p.IsInActive.Equals(false));
                var referedPackage = articlePackages.Where(p => p.Package_Articles.Any(i => i._Id.Equals(curItemID)));
                return referedPackage;
            }
            catch (Exception ex)
            {
                _logger.Error("Error Finding the package reference", ex);
                return null;
            }
        }
        public IEnumerable<IListableViewModel> GetPackageArticles(IArticle_Package Package)
        {
            try
            {
                IEnumerable<IListableViewModel> listablePackageArticles;
                if (Package != null && Package.Package_Articles.Any())
                {
                    var packageArticles = Package.Package_Articles.OfType<IArticle>()?.Where(p => p != null);
                    if (IsArticlePage && packageArticles != null && packageArticles.Any())
                    {
                        packageArticles = packageArticles.Where(i => !i._Id.ToString().Equals(curItemID.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    }
                    if (packageArticles != null && packageArticles.Any())
                    {
                        listablePackageArticles = packageArticles.Select(a => ArticleListableFactory.Create(a));
                        if (listablePackageArticles.Count() > 4 && IsArticlePage)
                        {
                            return listablePackageArticles.Take(4);
                        }
                        return listablePackageArticles;
                    }
                }
                return Enumerable.Empty<IListableViewModel>();
            }
            catch (Exception ex)
            {
                _logger.Error("Error Finding the package articles", ex);
                return Enumerable.Empty<IListableViewModel>();
            }
        }
    }
}