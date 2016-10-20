using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
    public interface IPersonalizedArticleSearchResults
    {
        IEnumerable<IPersonalizedArticle> Articles {get;set;}
        ILoadMore LoadMore { get; set; }
    }
}
