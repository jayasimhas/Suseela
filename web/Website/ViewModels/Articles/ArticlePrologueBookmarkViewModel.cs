using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.Services.Article;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
	[AutowireService(LifetimeScope.PerScope)]
	public class ArticlePrologueBookmarkViewModel : IArticlePrologueBookmarkViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IArticleService ArticleService;

		public ArticlePrologueBookmarkViewModel(
			ITextTranslator textTranslator,
			IRenderingItemContext articleRenderingContext,
			IAuthenticatedUserContext authenticatedUserContext,
			IIsSavedDocumentContext isSavedDocuementContext,
			ISiteRootContext siteRootContext,
			IArticleService articleService)
		{
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			ArticleService = articleService;

			Article = articleRenderingContext.Get<IArticle>();
			IsUserAuthenticated = authenticatedUserContext.IsAuthenticated;
			IsArticleBookmarked = IsUserAuthenticated && isSavedDocuementContext.IsSaved(Article._Id);
			BookmarkPublication = ArticleService.GetArticlePublicationName(Article);
            BookmarkTitle = Article?.Title;
            SalesforceId = IsUserAuthenticated ? isSavedDocuementContext.GetSalesforceId(Article._Id) : string.Empty;

		}

		public IArticle Article { get; set; }
		public bool IsUserAuthenticated { get; set; }
		public bool IsArticleBookmarked { get; set; }
		public string BookmarkText => TextTranslator.Translate("Bookmark");
		public string BookmarkTitle { get; }
		public string BookmarkPublication { get; }
		public string BookmarkedText => TextTranslator.Translate("Bookmarked");
        public string SalesforceId { get; set; }

    }
}