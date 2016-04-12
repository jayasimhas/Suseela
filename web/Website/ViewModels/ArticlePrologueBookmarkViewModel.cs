using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticlePrologueBookmarkViewModel : IArticlePrologueBookmarkViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly IRenderingItemContext ArticleRenderingContext;
	    protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IIsSavedDocumentContext IsSavedDocuementContext;

		public ArticlePrologueBookmarkViewModel(
			ITextTranslator textTranslator,
			IRenderingItemContext articleRenderingContext,
			ISignInViewModel signInViewModel,
            IAuthenticatedUserContext authenticatedUserContext,
			IIsSavedDocumentContext isSavedDocuementContext)
		{
			TextTranslator = textTranslator;
			ArticleRenderingContext = articleRenderingContext;
			SignInViewModel = signInViewModel;
            AuthenticatedUserContext = authenticatedUserContext;
			IsSavedDocuementContext = isSavedDocuementContext;
		}

		public IArticle Article => ArticleRenderingContext.Get<IArticle>();
		public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
		public bool IsArticleBookmarked => IsSavedDocuementContext.IsSaved(Article._Id);
		public string BookmarkText => TextTranslator.Translate("Bookmark");
		public string BookmarkedText => TextTranslator.Translate("Bookmarked");
		public ISignInViewModel SignInViewModel { get; set; }
	}
}