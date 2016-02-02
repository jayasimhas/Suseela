using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Jabberwocky.Glass.Factory.Attributes;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Search.Results.Facets;

namespace Velir.Search.Core.Refinements
{
	/// <summary>
	/// Interface to define how a refinement should affect search results.
	/// </summary>
	[GlassFactoryInterface]
	public interface ISearchRefinement
	{
		string RefinementLabel { get; }

		/// <summary>
		/// Gets the refinement key.
		/// </summary>
		/// <value>
		/// The refinement key.
		/// </value>
		string RefinementKey { get; }

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		/// <value>
		/// The name of the field.
		/// </value>
		string FieldName { get; }

		/// <summary>
		/// Gets the filter expression to be applied to a search query.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem;

		/// <summary>
		/// Gets a value indicating whether search results should be faceted by this field.
		/// </summary>
		/// <value>
		///   <c>true</c> if [facet on me]; otherwise, <c>false</c>.
		/// </value>
		bool FacetOnMe { get; }

		/// <summary>
		/// Gets the expression needed to facet on this field.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem;

		/// <summary>
		/// Gets the facet order.
		/// </summary>
		/// <param name="allValues">All values.</param>
		/// <param name="selectedValues">The selected values.</param>
		/// <returns></returns>
		IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues);
	}
}
