using System;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;

namespace Informa.Web.ViewModels
{
	public interface IListableViewModel : IListable, IArticleBookmarker
	{
		bool DisplayImage { get; set; }
		string PageTitle { get; set; }
        ISponsored_Content SponsoredContent { get; set; }
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
