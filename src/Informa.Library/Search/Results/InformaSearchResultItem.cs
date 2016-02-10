using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Informa.Library.Search.Results
{
    public class InformaSearchResultItem : SearchResultItem
    {
        [IndexField("IsSearchable")]
        public bool IsSearchable { get; set; }

        [IndexField("_latestversion")]
        public bool IsLatestVersion { get; set; }

        [IndexField("SearchDate")]
        public DateTime SearchDate { get; set; }
    }
}
