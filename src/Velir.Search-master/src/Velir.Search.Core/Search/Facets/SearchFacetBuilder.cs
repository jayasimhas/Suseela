using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Refinements;

namespace Velir.Search.Core.Search.Facets
{
	/// <summary>
	/// Implementation of IFacetBuilder which will return a list of Expressions
	/// representing the fields our search results should be faceted on.
	/// </summary>
	public class SearchFacetBuilder<T> : IFacetBuilder<T> where T : SearchResultItem
	{
		protected IEnumerable<ISearchRefinement> Facets { get; set; }

		public SearchFacetBuilder()
		{
			Facets = new List<ISearchRefinement>();
		}

		public SearchFacetBuilder(IEnumerable<ISearchRefinement> refinements)
		{
			Facets = refinements ?? new List<ISearchRefinement>();
		}

		/// <summary>
		/// Builds Expression functions for fields to facet by.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<Expression<Func<T, object>>> Build()
		{
			return Facets
				.Reverse()
				.Where(facet => facet.FacetOnMe)
				.Select(facet => facet.GetFacetFieldExpression<T>())
				.ToList();
		}
	}
}