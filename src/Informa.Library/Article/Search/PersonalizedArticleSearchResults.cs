namespace Informa.Library.Article.Search
{
    using System.Collections.Generic;

    public class PersonalizedArticleSearchResults : IPersonalizedArticleSearchResults
    {
        public IEnumerable<IPersonalizedArticle> Articles { get; set; }
        public ILoadMore LoadMore { get; set; }
    }
}
