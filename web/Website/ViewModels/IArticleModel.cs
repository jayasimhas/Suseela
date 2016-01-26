using System;
using System.Collections.Generic;
using Informa.Models.FactoryInterface;

namespace Informa.Web.ViewModels
{
    public interface IArticleModel : IListable
    {
        string Title { get; }
        string Sub_Title { get; }
        string Body { get; }
        IHierarchyLinks TaxonomyHierarchy { get; }
        IEnumerable<ILinkable> TaxonomyItems { get; }
        DateTime Date { get; }
        string Content_Type { get; }
        string Media_Type { get; }
        IEnumerable<IPersonModel> Authors { get; }
        string Category { get; }     
        IEnumerable<IListable> RelatedArticles { get; }  
        
    }
}