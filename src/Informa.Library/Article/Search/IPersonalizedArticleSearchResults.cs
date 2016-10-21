using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
    public interface IPersonalizedArticleSearchResults
    {
        IEnumerable<IPersonalizedArticle> PersonalizedArticles {get;set;}
        ILoadMore LoadMore { get; set; }
        long TotalResults { get; set; }
        IEnumerable<IArticle> Articles { get; set; }
    }
}
