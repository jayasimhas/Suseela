using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Page;
using Informa.Library.Services.Article;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.StringUtils;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
    [AutowireService(LifetimeScope.PerScope)]
    public class ArticleListItemModelFactory : IArticleListItemModelFactory
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleService ArticleService;
        protected readonly IBylineMaker ByLineMaker;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IIsSavedDocumentContext IsSavedDocumentContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IPageItemContext PageItemContext;
        protected IDCDReader DcdReader;

        public ArticleListItemModelFactory(
            ISitecoreContext sitecoreContext,
            IArticleSearch articleSearch,
            IArticleService articleService,
            IBylineMaker byLineMaker,
            IAuthenticatedUserContext authenticatedUserContext,
            IIsSavedDocumentContext isSavedDocumentContext,
            ITextTranslator textTranslator,
            IPageItemContext pageItemContext,
            IDCDReader dcdReader)
        {
            SitecoreContext = sitecoreContext;
            ArticleSearch = articleSearch;
            ArticleService = articleService;
            ByLineMaker = byLineMaker;
            AuthenticatedUserContext = authenticatedUserContext;
            IsSavedDocumentContext = isSavedDocumentContext;
            TextTranslator = textTranslator;
            PageItemContext = pageItemContext;
            DcdReader = dcdReader;
        }

        public IListableViewModel Create(IArticle article)
        {
            if (article == null)
                return null;

            var image = article.Featured_Image_16_9?.Src;
            var curPage = PageItemContext.Get<I___BasePage>();

            var model = new ArticleListItemModel();
            model.DisplayImage = !string.IsNullOrWhiteSpace(image);
            model.ListableAuthorByLine = ByLineMaker.MakeByline(article.Authors);
            model.ListableDate = article.Actual_Publish_Date;
            model.ListableImage = image;
            model.ListableSummary = ArticleService.GetArticleSummary(article);
            model.ListableTitle = HttpUtility.HtmlDecode(article.Title);
            model.ListablePublication = ArticleService.GetArticlePublicationName(article);
            model.ListableTopics = ArticleService.GetLinkableTaxonomies(article);
            model.ListableType = ArticleService.GetMediaTypeIconData(article)?.MediaType;
            model.ListableUrl = new Link { Url = article._Url, Text = article.Title };
            model.LinkableText = article.Content_Type?.Item_Name;
            model.LinkableUrl = article._Url;
            model.ID = article._Id;
            model.IsUserAuthenticated = AuthenticatedUserContext.IsAuthenticated;
            model.IsArticleBookmarked = IsSavedDocumentContext.IsSaved(article._Id);
            model.BookmarkText = TextTranslator.Translate("Bookmark");
            model.BookmarkedText = TextTranslator.Translate("Bookmarked");
            model.PageTitle = curPage?.Title;

            if (curPage._TemplateId.Equals(ICompany_PageConstants.TemplateId.Guid))
            {
                var recordNumber = HttpContext.Current.Request.Url.Segments.Last();
                var company = DcdReader.GetCompanyByRecordNumber(recordNumber);
                model.PageTitle = company.Title;
            }

            model.SalesforceId = IsSavedDocumentContext.GetSalesforceId(article._Id);
            return model;
        }

        /// <summary>
        /// Returns personalized article item
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public IPersonalizedArticle CreatePersonalizedArticle(IArticle article)
        {
            if (article == null)
                return null;

            var image = article.Featured_Image_16_9?.Src;
            var curPage = PageItemContext.Get<I___BasePage>();

            var model = new PersonalizedArticleListItem();
            model.ListableAuthorByLine = ByLineMaker.MakeByline(article.Authors);
            model.ListableDate = article.Actual_Publish_Date.ToString("dd MMM yyyy");
            model.ListableImage = image;
            model.ListableSummary = ArticleService.GetArticleSummary(article);
            model.ListableTitle = HttpUtility.HtmlDecode(article.Title);
            model.ListablePublication = ArticleService.GetArticlePublicationName(article);
            model.ListableTopics = ArticleService.GetPersonalizedLinkableTaxonomies(article);
            model.ListableType = article.Media_Type?.Media_Type_Icon?.Src;//ArticleService.GetMediaTypeIconData(article)?.MediaType;
            model.LinkableText = article.Content_Type?.Item_Name;
            model.LinkableUrl = article._Url;
            model.ID = article._Id;
            model.IsUserAuthenticated = AuthenticatedUserContext.IsAuthenticated;
            model.IsArticleBookmarked = IsSavedDocumentContext.IsSaved(article._Id);
            model.BookmarkText = TextTranslator.Translate("Bookmark");
            model.BookmarkedText = TextTranslator.Translate("Bookmarked");

            if (curPage._TemplateId.Equals(ICompany_PageConstants.TemplateId.Guid))
            {
                var recordNumber = HttpContext.Current.Request.Url.Segments.Last();
                var company = DcdReader.GetCompanyByRecordNumber(recordNumber);
            }

            return model;
        }

        public IListableViewModel Create(Guid articleId)
        {
            return Create(SitecoreContext.GetItem<IArticle>(articleId));
        }

        public IListableViewModel Create(string articleNumber)
        {
            IArticleSearchFilter filter = ArticleSearch.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            filter.PageSize = 1;
            var results = ArticleSearch.Search(filter);

            return Create(results.Articles.FirstOrDefault());
        }
    }
}