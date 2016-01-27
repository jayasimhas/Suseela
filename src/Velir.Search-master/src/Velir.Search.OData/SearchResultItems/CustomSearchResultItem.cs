using System.Collections.Generic;
using System.Runtime.Serialization;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Velir.Search.OData.SearchResultItems
{
	[DataContract]
	public class CustomSearchResultItem : SearchResultItem
	{
		[DataMember]
		public virtual string ID
		{
			get { return ItemId.Guid.ToString(); }
		}

		[IgnoreDataMember]
		public override IEnumerable<ID> Semantics { get; set; }
	}
}
