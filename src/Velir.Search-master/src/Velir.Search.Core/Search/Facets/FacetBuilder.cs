using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.SearchResultItems;

namespace Velir.Search.Core.Search.Facets
{
	public class FacetBuilder<T> : IFacetBuilder<T> where T : CustomSearchResultItem
	{
		public IList<Expression<Func<T, object>>> Fields { get; set; }
 
		public FacetBuilder()
		{
			Fields = new List<Expression<Func<T, object>>>();
		} 

		public IEnumerable<Expression<Func<T, object>>> Build()
		{
			return Fields;
		}
	}
}
