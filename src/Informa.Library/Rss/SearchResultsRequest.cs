using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Rss
{
    public class SearchResultsRequest
    {
        public string page { get; set; }
        public string pageId { get; set; }
        public string perPage { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }

    }

}
