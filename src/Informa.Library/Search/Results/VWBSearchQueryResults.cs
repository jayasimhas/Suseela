using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Search.Results
{
    public class VWBSearchQueryResults
    {
        public Dictionary<string, string> Taxonomies { get; set; }
        public Dictionary<string, string> Authors { get; set; }
        public Dictionary<string, string> ContentTypes { get; set; }
        public Dictionary<string, string> MediaTypes { get; set; }
    }
}
