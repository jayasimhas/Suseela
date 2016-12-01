using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Collections.Generic;

namespace Informa.Web.Models
{
    public class Section : ISection
    {
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public IList<string> TaxonomyIds { get; set; }

        // Adding additional properties for IPMP-906
        public ILoadMore LoadMore { get; set; }
        public IEnumerable<IPersonalizedArticle> Articles { get; set; }
        public string SectionTitle { get; set; }
    }
}
