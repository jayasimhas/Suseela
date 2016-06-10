using System;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels
{
	public interface IListableViewModel : IListable, IArticleBookmarker
	{
		bool DisplayImage { get; set; }
		string PageTitle { get; set; }
	}

    public interface IArticleBookmarker
    {
        bool IsUserAuthenticated { get; set; }
        bool IsArticleBookmarked { get; set; }
        string BookmarkText { get; set; }
        string BookmarkedText { get; set; }
				Guid ID { get; }
    }
}
