using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
    public class HierarchyLinks
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public IEnumerable<HierarchyLinks> Children { get; set; } 
    }
}