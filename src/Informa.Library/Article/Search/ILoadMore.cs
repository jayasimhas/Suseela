namespace Informa.Library.Article.Search
{
    public interface ILoadMore
    {
        bool DisplayLoadMore { get; set; }
        string LoadMoreLinkText { get; set; }
        string LoadMoreLinkUrl { get; set; }
    }
}
