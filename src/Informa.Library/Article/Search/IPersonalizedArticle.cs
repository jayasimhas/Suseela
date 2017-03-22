using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using System;
using System.Collections.Generic;

namespace Informa.Library.Article.Search
{
    public interface IPersonalizedArticle : ILinkable, IArticleBookmarker
    {
        string ListableAuthorByLine { get; }
        string ListableDate { get; }
        string ListableImage { get; }
        string ListableSummary { get; }
        string ListableTitle { get; }
        string ListablePublication { get; }
        IEnumerable<ILinkable> ListableTopics { get; }
        string ListableType { get; }
        bool isSonsoredBy { get; }
        string SponsoredByTitle { get; }
        string SponsoredByLogo { get; }
        string SponsoredLink { get; }
    }

    public interface ILinkable
    {
        string LinkableText { get; }
        string LinkableUrl { get; }
    }
    public interface IArticleBookmarker
    {
        bool IsUserAuthenticated { get; }
        bool IsArticleBookmarked { get; }
        string BookmarkText { get; }
        string BookmarkedText { get; }
        Guid ID { get; }
        string SalesforceId { get; }
    }

    public class Linkable : ILinkable
    {
        public string LinkableText { get; set; }
        public string LinkableUrl { get; set; }
    }
}
