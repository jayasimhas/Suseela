using Informa.Library.ViewModels.Account;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public interface IArticlePrologueBookmarkViewModel
	{
        IArticle Article { get; }
        bool IsUserAuthenticated { get; }
		bool IsArticleBookmarked { get; }
		string BookmarkedText { get; }
		string BookmarkText { get; }
		string BookmarkTitle { get; }
		string BookmarkPublication { get; }
	}
}
