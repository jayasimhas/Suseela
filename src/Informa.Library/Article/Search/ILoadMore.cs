namespace Informa.Library.Article.Search
{
    using System.Collections.Generic;
    public interface ILoadMore
    {
        bool DisplayLoadMore { get; }
        string LoadMoreLinkText { get; }
        string LoadMoreLinkUrl { get; }
        string LatestFromText { get; }
        int PageNo { get; }
        int PageSize { get; }
        IList<string> TaxonomyIds {get;}
    }
}
