using System;
using System.Collections.Generic;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
    public interface IArticleModel : IListableViewModel
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
        IEnumerable<IGlassBase> KeyDocuments { get; }
        bool IsUserAuthenticated { get; }
        bool IsArticleBookmarked { get; }
        string BookmarkedText { get; }
        string BookmarkText { get; }
    }
}