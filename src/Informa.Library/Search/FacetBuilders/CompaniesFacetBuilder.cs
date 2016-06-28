using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Informa.Library.Search.Results;
using Velir.Search.Core.Facets;

namespace Informa.Library.Search.FacetBuilders
{
	public class CompaniesFacetBuilder : IFacetBuilder<InformaSearchResultItem>
	{
		public IEnumerable<Expression<Func<InformaSearchResultItem, object>>> Build()
		{
			yield return x => x.CompaniesFacet;
		}
	}
}
