using System.Collections.Generic;

namespace Velir.Search.Core.Search.Results.Facets
{
	public class FacetGroup
	{
		public virtual string Id { get; set; }
		public virtual string Label { get; set; }
		public virtual IEnumerable<FacetResultValue> Values { get; set; }
	}
}
