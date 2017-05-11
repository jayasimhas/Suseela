using System;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;

namespace Informa.Web.ViewModels
{
	public interface IListableViewModel : IListable, IArticleBookmarker
	{
		bool DisplayImage { get; set; }
		string PageTitle { get; set; }
        ISponsored_Content SponsoredContent { get; set; }
        MediaTypeIconData MediaTypeIconData { get; set; }

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
