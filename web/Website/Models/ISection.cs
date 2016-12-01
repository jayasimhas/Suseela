using System.Collections.Generic;

namespace Informa.Web.Models
{
    public interface ISection
    {

        string ChannelId { get; set; }
        string ChannelName { get; set; }
        IList<string> TaxonomyIds { get; set; }
    }
}