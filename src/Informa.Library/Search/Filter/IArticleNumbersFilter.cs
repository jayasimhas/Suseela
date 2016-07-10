using System;
using System.Collections.Generic;

namespace Informa.Library.Search.Filter
{
    public interface IArticleNumbersFilter
    {
        IList<string> ArticleNumbers { get; set; }
    }

    public interface IReferencedArticleFilter
    {
        Guid ReferencedArticle { get; set; }
    }
}
