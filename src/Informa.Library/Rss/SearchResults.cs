using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Search.Results;

namespace Informa.Library.Rss
{
    public class SearchResults
    {
        public SearchResultsRequest request { get; set; }
        public string totalResults { get; set; }
        public List<InformaSearchResultItem> results { get; set; }
    }
}
