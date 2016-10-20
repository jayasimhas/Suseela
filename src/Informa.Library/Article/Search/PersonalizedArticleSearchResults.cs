namespace Informa.Library.Article.Search
{
    using Models.Informa.Models.sitecore.templates.User_Defined.Pages;
    using System.Collections.Generic;

    public class PersonalizedArticleSearchResults : IPersonalizedArticleSearchResults
    {
        public IEnumerable<IPersonalizedArticle> PersonalizedArticles { get; set; }
        public ILoadMore LoadMore { get; set; }

        public long TotalResults { get; set; }

        public IEnumerable<IArticle> Articles { get; set; }
    }
}
