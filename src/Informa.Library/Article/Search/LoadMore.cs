namespace Informa.Library.Article.Search
{
    using System;
    using System.Collections.Generic;

    public class LoadMore : ILoadMore
    {
        public bool DisplayLoadMore { get; set; }
        public string LatestFromText { get; set; }
        public string LoadMoreLinkText { get; set; }
        public string LoadMoreLinkUrl { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SeeAllText { get; set; }
        public string SeeAllLink { get; set; }
        public string CurrentlyViewingText { get; set; }
        public IList<string> TaxonomyIds { get; set; }
    }
}
