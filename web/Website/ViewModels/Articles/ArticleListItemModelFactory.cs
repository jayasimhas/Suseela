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
            {
                return null;
            }

            var publication = article.Crawl<ISite_Root>();
            var image = article.Featured_Image_16_9?.Src;

            var model = new ArticleListItemModel
            {
                DisplayImage = !string.IsNullOrWhiteSpace(image),
                ListableAuthorByLine = ByLineMaker.MakeByline(article.Authors),
                ListableDate = article.Actual_Publish_Date,
                ListableImage = image,
                ListableSummary = ArticleService.GetArticleSummary(article),
                ListableTitle = HttpUtility.HtmlDecode(article.Title),
                ListablePublication = publication.Publication_Name,
                ListableTopics = ArticleService.GetLinkableTaxonomies(article),
                ListableType = ArticleService.GetMediaTypeIconData(article)?.MediaType,
                ListableUrl = new Link { Url = article._Url, Text = article.Title },
                LinkableText = article.Content_Type?.Item_Name,
                LinkableUrl = article._Url,
                ID = article._Id,
                IsUserAuthenticated = AuthenticatedUserContext.IsAuthenticated,
                IsArticleBookmarked = IsSavedDocumentContext.IsSaved(article._Id),
                BookmarkText = TextTranslator.Translate("Bookmark"),
                BookmarkedText = TextTranslator.Translate("Bookmarked"),
                PageTitle = PageItemContext.Get<I___BasePage>()?.Title
            };

            if (PageItemContext.Get<ICompany_Page>() != null)
            {
                var recordNumber = HttpContext.Current.Request.Url.Segments.Last();
                var company = DcdReader.GetCompanyByRecordNumber(recordNumber);
                model.PageTitle = company.Title;
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