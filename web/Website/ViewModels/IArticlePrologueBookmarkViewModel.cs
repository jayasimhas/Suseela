namespace Informa.Web.ViewModels
{
	public interface IArticlePrologueBookmarkViewModel
	{
		bool IsUserAuthenticated { get; }
		bool IsArticleBookmarked { get; }
		string BookmarkedText { get; }
		string BookmarkText { get; }
		ISignInViewModel SignInViewModel { get; }
	}
}
