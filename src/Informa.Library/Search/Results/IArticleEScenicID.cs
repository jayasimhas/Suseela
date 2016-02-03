using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
    public interface IArticleEScenicID
    {
        [IndexField(IArticleConstants.Escenic_IDFieldName)]
        string EScenicID { get; set; }
    }
}
