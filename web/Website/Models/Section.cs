using System.Collections.Generic;

namespace Informa.Web.Models
{
    public class Section : ISection
    {
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public IList<string> TaxonomyIds { get; set; }
    }
}