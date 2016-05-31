using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Taxonomy {
    public class HierarchyLinks : IHierarchyLinks {
        public string Text { get; set; }
        public string Url { get; set; }
        public IEnumerable<IHierarchyLinks> Children { get; set; }
		public string Path { get; set; }
    }
}
