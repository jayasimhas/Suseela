using Informa.Library.Article.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.SponsoredContent
{
    public interface ISponsoredContent
    {
        IListableViewModel ListableItems { get; set; }
        ArticleViewType ArticleViewType { get; set; }
    }

    public enum ArticleViewType
    {
        LatestNews = 1,
        FeaturedArticle = 2
    }
}