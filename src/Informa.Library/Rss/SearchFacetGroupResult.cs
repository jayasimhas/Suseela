using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Rss
{
    public class SearchFacetGroupResult
    {
        public string id { get; set; }
        public string label { get; set; }
        public List<SearchFacetResult> values { get; set; }
    }
}
