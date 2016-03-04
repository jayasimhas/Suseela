using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace Informa.Library.Search.Results
{
    public class GeneralContentResult : SearchResultItem
    {
        [IndexField(IGeneral_Content_PageConstants.Exclude_From_Google_SearchFieldName)]
        public bool ExcludeFromGoogleSearch { get; set; }
    }
}
