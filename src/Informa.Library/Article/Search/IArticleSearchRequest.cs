namespace Informa.Library.Article.Search
{
    using System.Collections.Generic;

    public interface IArticleSearchRequest
    {
        int PageNo { get; }
        int PageSize { get; }
        IList<string> TaxonomyIds { get; }
        string ChannelId { get; }
    }
}
