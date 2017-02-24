using Informa.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Article.Models.Article.LatestPublishedStories
{
    public class LatestPublishedStory
    {
        public IEnumerable<IListableViewModel> News { get; set; }
        public IList<string> Authors { get; set; }
        public IEnumerable<Guid> Topics { get; set; }
        public int MaxStoriesToDisplay { get; set; }
        public string PublicationName { get; set; }
        public IEnumerable<Guid> ContentType { get; set; }
        public IEnumerable<Guid> MediaType { get; set; }
        public string LoadMoreText { get; set; }
        public bool IsDisplayDate { get; set; }
        public string LatestStoriesComponentTitle { get; set; }
        public bool IsDisableBackground { get; set; }
    }
}