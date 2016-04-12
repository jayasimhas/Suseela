using System;
using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.ContentSearch;

namespace Informa.Library.Search.Results
{
    public interface IReferencedArticles
    {
        [IndexField(IArticleConstants.Referenced_ArticlesFieldName)]
        List<Guid> ReferencedArticles { get; set; }
    }
}