using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.PerScope)]
	public class ArticlePrologueBookmarkViewModel : IArticlePrologueBookmarkViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		
		public ArticlePrologueBookmarkViewModel(
			ITextTranslator textTranslator,
			IRenderingItemContext articleRenderingContext,
			IAuthenticatedUserContext authenticatedUserContext,
			IIsSavedDocumentContext isSavedDocuementContext)
		{
			TextTranslator = textTranslator;

			Article = articleRenderingContext.Get<IArticle>();
			IsUserAuthenticated = authenticatedUserContext.IsAuthenticated;
			IsArticleBookmarked = IsUserAuthenticated && isSavedDocuementContext.IsSaved(Article._Id);
		}

		public IArticle Article { get; set; }
		public bool IsUserAuthenticated { get; set; }
		public bool IsArticleBookmarked { get; set; }
		public string BookmarkText => TextTranslator.Translate("Bookmark");
		public string BookmarkedText => TextTranslator.Translate("Bookmarked");
	}
}