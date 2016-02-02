using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
    public class HierarchyLinks : IHierarchyLinks
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public IEnumerable<IHierarchyLinks> Children { get; set; } 
    }

    public interface IHierarchyLinks
    {
        string Text { get; }
        string Url { get; }
        IEnumerable<IHierarchyLinks> Children { get; } 
    }
}