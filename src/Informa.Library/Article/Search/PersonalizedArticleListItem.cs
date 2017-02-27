namespace Informa.Library.Article.Search
{
    using System;
    using System.Collections.Generic;
    public class PersonalizedArticleListItem : IPersonalizedArticle
    {
        public string BookmarkedText { get; set; }
        public string BookmarkText { get; set; }
        public Guid ID { get; set; }
        public bool IsArticleBookmarked { get; set; }
        public bool IsUserAuthenticated { get; set; }
        public string LinkableText { get; set; }
        public string LinkableUrl { get; set; }
        public string ListableAuthorByLine { get; set; }
        public string ListableDate { get; set; }
        public string ListableImage { get; set; }
        public string ListablePublication { get; set; }
        public string ListableSummary { get; set; }
        public string ListableTitle { get; set; }
        public IEnumerable<ILinkable> ListableTopics { get; set; }
        public string ListableType { get; set; }
        public string SalesforceId { get; set; }
    }
}
