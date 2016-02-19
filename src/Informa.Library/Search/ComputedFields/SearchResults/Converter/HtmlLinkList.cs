using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Search.ComputedFields.SearchResults.Converter
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
