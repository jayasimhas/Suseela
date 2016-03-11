using Informa.Library.Article;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticlePrologueBookmarkViewModel : IArticlePrologueBookmarkViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly IRenderingItemContext ArticleRenderingContext;
		protected readonly IManageSavedDocuments ManageSavedDocuments;
	    protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

		public ArticlePrologueBookmarkViewModel(
			ITextTranslator textTranslator,
			IRenderingItemContext articleRenderingContext,
            IManageSavedDocuments manageSavedDocuments,
			ISignInViewModel signInViewModel,
            IAuthenticatedUserContext authenticatedUserContext)
		{
			TextTranslator = textTranslator;
			ArticleRenderingContext = articleRenderingContext;
            ManageSavedDocuments = manageSavedDocuments;
			SignInViewModel = signInViewModel;
            AuthenticatedUserContext = authenticatedUserContext;
		}

		public IArticle Article => ArticleRenderingContext.Get<IArticle>();
		public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
		public bool IsArticleBookmarked => ManageSavedDocuments.IsBookmarked(AuthenticatedUserContext.User, Article._Id);
		public string BookmarkText => TextTranslator.Translate("Bookmark");
		public string BookmarkedText => TextTranslator.Translate("Bookmarked");
		public ISignInViewModel SignInViewModel { get; set; }
	}
}