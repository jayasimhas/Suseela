using System;
using System.Collections.Generic;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Search
{
    public class ArticleSearchResultItem : SearchResultItem, ITaxonomySearchResults, IArticleNumber, IArticleEScenicID, IReferencedArticles, IArticlePublicationResults, IArticleAuthorResults
    {
        [IndexField("_latestversion")]
        public bool IsLatestVersion { get; set; }

        public List<Guid> Taxonomies { get; set; }
        [IndexField(IArticleConstants.Actual_Publish_DateFieldName)]
        public DateTime ActualPublishDate { get; set; }
        public string ArticleNumber { get; set; }
        [IndexField("searchpublicationtitle_s")]
        public string PublicationTitle { get; set; }
        public long ArticleIntegerNumber { get; set; }
        public string EScenicID { get; set; }
        [IndexField("legacy_article_url_s")]
        public string LegacyArticleUrl { get; set; }
        public IList<string> AuthorGuid { get; set; }
        #region Implementation of IReferencedArticles
        public List<Guid> ReferencedArticles { get; set; }

        #endregion
    }
}


