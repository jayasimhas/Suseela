namespace Informa.Library.Article.Search
{
    public interface ILoadMore
    {
        bool DisplayLoadMore { get; }
        string LoadMoreLinkText { get; }
        string LoadMoreLinkUrl { get; }
    }
}
