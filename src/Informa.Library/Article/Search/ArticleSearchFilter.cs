using System;
using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
    public class ArticleSearchFilter : IArticleSearchFilter
    {
        public IList<Guid> ExcludeManuallyCuratedItems { get; set; }
        public IList<Guid> Taxonomies { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string PublicationTitle { get; set; }
        public IList<Guid> TaxonomyIds { get; set; }
        public IList<string> ArticleNumbers { get; set; }
        public string EScenicID { get; set; }
        public Guid ReferencedArticle { get; set; }
        public IList<string> PublicationNames { get; set; }
        public IList<string> AuthorNames { get; set; }
        public IList<string> CompanyRecordNumbers { get; set; }
    }
}
