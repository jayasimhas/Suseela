using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Velir.Search.Core.Search.Results.Facets
{
	[DataContract]
	public class FacetResultValue
	{
		public FacetResultValue()
		{
			Name = string.Empty;
			Count = 0;
			Selected = false;
			Sublist = new List<FacetResultValue>();
		}

		public FacetResultValue(string name, int count, bool selected = false)
		{
			Name = name;
			Count = count;
			Selected = selected;
			Sublist = new List<FacetResultValue>();
		}

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Count { get; set; }

		[DataMember]
		public bool Selected { get; set; }

		[IgnoreDataMember]
		public IEnumerable<FacetResultValue> Sublist { get; set; }
	}
}
