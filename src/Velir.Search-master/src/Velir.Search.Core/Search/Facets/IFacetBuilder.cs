using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;

namespace Velir.Search.Core.Search.Facets
{
	public interface IFacetBuilder<T> where T : SearchResultItem
	{
		IEnumerable<Expression<Func<T, object>>> Build();
	}
}