namespace Informa.Library.Article.Search
{
    public interface ILoadMore
    {
        bool DisplayLoadMore { get; }
        string LoadMoreLinkText { get; }
        string LoadMoreLinkUrl { get; }
        string LatestFromText { get; }
        int PageNo { get; }
        int PageSize { get; }
    }
}
