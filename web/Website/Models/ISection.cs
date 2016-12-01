using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Collections.Generic;

namespace Informa.Web.Models
{
    public interface ISection
    {
        string ChannelId { get; set; }
        string ChannelName { get; set; }
        IList<string> TaxonomyIds { get; set; }

        // Adding additional properties for IPMP-906
        IEnumerable<IPersonalizedArticle> Articles { get; set; }
        ILoadMore LoadMore { get; set; }
        string SectionTitle { get; set; }
    }
}
