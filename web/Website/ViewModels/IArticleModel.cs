using System;
using System.Collections.Generic;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels
{
    public interface IArticleModel
    {
        string Title { get; }
        string Sub_Title { get; }
        string Body { get; }
        //IEnumerable<HierarchyLinks> TaxonomyItems { get; }
        IEnumerable<ILinkable> TaxonomyItems { get; }
        DateTime Date { get; }
        string Content_Type { get; }
        string Media_Type { get; }
        IEnumerable<IAuthorModel> Authors { get; }
        string Category { get; }          
    }
}