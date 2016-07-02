using System.Collections.Generic;

namespace Informa.Library.Search.ComputedFields.SearchResults.Converter.DisplayTaxonomy
{
    public class HtmlLinkList
    {
        public HtmlLinkList()
        {
            Links = new List<HtmlLink>();
        }

        public List<HtmlLink> Links { get; set; } 
    }
}
