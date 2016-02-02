using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
    public interface IArticleNumber
    {
        [IndexField(IArticleConstants.Article_NumberFieldName)]
        string ArticleNumber { get; set; }
    }
}
