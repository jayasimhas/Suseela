using Informa.Library.Article;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticlePrologueBookmarkViewModel : IArticlePrologueBookmarkViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly IRenderingItemContext ArticleRenderingContext;
		protected readonly IUserArticleBookmarkedContext UserArticleBookmarkedContext;

		public ArticlePrologueBookmarkViewModel(
			ITextTranslator textTranslator,
			IRenderingItemContext articleRenderingContext,
			IUserArticleBookmarkedContext userArticleBookmarkedContext,
			ISignInViewModel signInViewModel)
		{
			TextTranslator = textTranslator;
			ArticleRenderingContext = articleRenderingContext;
			UserArticleBookmarkedContext = userArticleBookmarkedContext;
			SignInViewModel = signInViewModel;
		}

		public IArticle Article => ArticleRenderingContext.Get<IArticle>();
		public bool IsUserAuthenticated => false;
		public bool IsArticleBookmarked => UserArticleBookmarkedContext.IsBookmarked(Article._Id);
		public string BookmarkText => TextTranslator.Translate("Bookmark");
		public string BookmarkedText => TextTranslator.Translate("Bookmarked");
		public ISignInViewModel SignInViewModel { get; set; }
	}
}