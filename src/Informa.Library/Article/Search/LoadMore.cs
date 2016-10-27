﻿using System;

namespace Informa.Library.Article.Search
{
    public class LoadMore : ILoadMore
    {
        public bool DisplayLoadMore { get; set; }
        public string LatestFromText { get; set; }
        public string LoadMoreLinkText { get; set; }
        public string LoadMoreLinkUrl { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        
    }
}
