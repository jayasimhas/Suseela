using System;
using System.Collections.Generic;

namespace Informa.Library.VirtualWhiteboard.Models
{
    public class IssueModel
    {
        public string Vertical { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Notes { get; set; }
        public IEnumerable<Guid> ArticleIds { get; set; }
    }
}
