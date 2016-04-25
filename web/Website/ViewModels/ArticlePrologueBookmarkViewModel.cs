using System;
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
			ISignInViewModel signInViewModel,
			IAuthenticatedUserContext authenticatedUserContext,
			IIsSavedDocumentContext isSavedDocuementContext)
		{
			TextTranslator = textTranslator;
			SignInViewModel = signInViewModel;
			
			_article = new Lazy<IArticle>(articleRenderingContext.Get<IArticle>);
			_isAuthenticated = new Lazy<bool>(authenticatedUserContext.IsAuthenticated);
			_isArticleBookmarked = new Lazy<bool>(IsUserAuthenticated && isSavedDocuementContext.IsSaved(Article._Id));
		}

		private readonly Lazy<IArticle> _article; 
		public IArticle Article => _article.Value;

		private readonly Lazy<bool> _isAuthenticated; 
		public bool IsUserAuthenticated => _isAuthenticated.Value;

		private readonly Lazy<bool> _isArticleBookmarked; 
		public bool IsArticleBookmarked => _isArticleBookmarked.Value;
		public string BookmarkText => TextTranslator.Translate("Bookmark");
		public string BookmarkedText => TextTranslator.Translate("Bookmarked");
		public ISignInViewModel SignInViewModel { get; set; }
	}
}