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
        public string ContentType { get; set; }
        public string MediaType { get; set; }
        public string LoadMoreText { get; set; }
    }
}