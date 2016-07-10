using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Taxonomy {
    public interface IHierarchyLinks {
        string Text { get; }
        string Url { get; }
        IEnumerable<IHierarchyLinks> Children { get; }
		string Path { get; set; }
    }
}
