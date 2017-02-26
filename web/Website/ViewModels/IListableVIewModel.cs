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
        bool IsUserAuthenticated { get; }
        bool IsArticleBookmarked { get; }
        string BookmarkText { get; }
        string BookmarkedText { get; }
		Guid ID { get; }
        string SalesforceId { get; }
    }
}
