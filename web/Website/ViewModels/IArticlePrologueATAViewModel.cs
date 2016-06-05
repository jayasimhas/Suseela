using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public interface IArticlePrologueATAViewModel
    {
        IArticle Article { get; }
        bool IsUserAuthenticated { get; }
		bool IsArticleBookmarked { get; }
		string BookmarkedText { get; }
		string BookmarkText { get; }
		ISignInViewModel SignInViewModel { get; }
	}
}
