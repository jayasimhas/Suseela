namespace Informa.Library.Article.Search
{
    using System.Collections.Generic;

    public class ArticleSearchRequest : IArticleSearchRequest
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public IList<string> TaxonomyIds { get; set; }
        public string ChannelId { get; set; }
    }
}
