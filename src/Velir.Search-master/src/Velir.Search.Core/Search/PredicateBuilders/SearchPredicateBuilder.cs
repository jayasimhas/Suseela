using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Models;
using Velir.Search.Core.Page;
using Velir.Search.Core.Reference;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Rules.Parser;
using Velir.Search.Core.Util;

namespace Velir.Search.Core.Search.PredicateBuilders
{
	/// <summary>
	/// PredicateBuilder class for Search and Listing pages.
	/// Applies proper filtering based on Listing and Refinement configurations in Sitecore.
	/// </summary>
	public class SearchPredicateBuilder<T> : AbstractPredicateBuilder<T> where T : SearchResultItem
	{
		private const float bigSlopFactor = 100000; // See http://wiki.apache.org/solr/SolrRelevancyCookbook "Term Proximity"

		protected IDictionary<string, ISearchRefinement> AvailableFacets { get; set; }
		protected IList<KeyValuePair<string, string[]>> SelectedRefinements { get; set; }

		public SearchPredicateBuilder(ISearchPageParser pageParser, ISearchRequest request = null) : base(pageParser.RuleParser)
		{
			if (pageParser != null && pageParser.ListingConfiguration != null)
			{
				AddRules(pageParser.ListingConfiguration.Hidden_Expression);	
			}
			
			if (request != null)
			{
				var validRefinements = request.GetRefinements();

				BuildFacetDictionary(validRefinements);
				BuildRefinementPairList(request.QueryParameters);
			}
		}

		private void BuildFacetDictionary(IEnumerable<ISearchRefinement> validRefinements)
		{
			AvailableFacets = new Dictionary<string, ISearchRefinement>();

			if (validRefinements != null)
			{
				foreach (var facet in validRefinements)
				{
					if (!AvailableFacets.ContainsKey(facet.RefinementKey))
					{
						AvailableFacets.Add(facet.RefinementKey, facet);
					}
				}
			}
		}

		private void BuildRefinementPairList(IDictionary<string, string> querystring)
		{
			SelectedRefinements = new List<KeyValuePair<string, string[]>>();

			if (querystring != null)
			{
				foreach (var pair in querystring)
				{
					string[] values = HttpUtility.HtmlDecode(pair.Value.Trim())
						.Split(SiteSettings.ValueSeparator)
						.Select(SearchEncoder.EncodeValue)
						.ToArray();
					SelectedRefinements.Add(new KeyValuePair<string, string[]>(pair.Key, values));
				}
			}
		}

		/// <summary>
		/// Builds the entire filter expression for the current search.
		/// </summary>
		/// <returns></returns>
		public override Expression<Func<T, bool>> Build()
		{
			var predicate = base.Build();

			foreach (var refinement in SelectedRefinements)
			{
				Expression<Func<T, bool>> clause = null;

				if (refinement.Key == SiteSettings.QueryString.QueryKey || refinement.Key == SiteSettings.QueryString.AllQueryKey)
				{
					clause = AddAllClause(refinement.Value);
				}
				else if (refinement.Key == SiteSettings.QueryString.AnyQueryKey)
				{
					clause = AddAnyClause(refinement.Value.SelectMany(str => str.Split(' ')).Select(str => str.Trim()).ToArray());
				}
				else if (refinement.Key == SiteSettings.QueryString.ExactQueryKey)
				{
					clause = AddExactClause(refinement.Value);
				}
				else if (refinement.Key == SiteSettings.QueryString.NoneQueryKey)
				{
					clause = AddNoneClause(refinement.Value.SelectMany(str => str.Split(' ')).Select(str => str.Trim()).ToArray());
				}
				else
				{
					clause = AddFacetSelection(refinement);
				}
				
				// apply clause to full predicate
				if (clause != null)
				{
					predicate = predicate.And(clause);
				}
			}

			return predicate;
		}

		/// <summary>
		/// Adds the facet selection.  Handles all generic facet selections
		/// </summary>
		/// <param name="refinement">The refinement.</param>
		/// <returns></returns>
		protected virtual Expression<Func<T, bool>> AddFacetSelection(KeyValuePair<string, string[]> refinement)
		{
			ISearchRefinement facet = AvailableFacets.ContainsKey(refinement.Key) ? AvailableFacets[refinement.Key] : null;
			if (facet != null)
			{
				return facet.GetFilterExpression<T>(refinement.Value);
			}

			return null;
		}

		/// <summary>
		/// Adds the none clause.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		protected virtual Expression<Func<T, bool>> AddNoneClause(string[] value)
		{
			var predicate = PredicateBuilder.False<T>();
			predicate = value.Aggregate(predicate, (current, s) => current.Or(x => x.Content.Equals(s)));

			return predicate.Not();
		}

		/// <summary>
		/// Adds the exact clause.
		/// </summary>
		/// <param name="terms">The value.</param>
		/// <returns></returns>
		protected virtual Expression<Func<T, bool>> AddExactClause(string[] terms)
		{
			var predicate = PredicateBuilder.False<T>();

			return terms.Aggregate(predicate, (current, term) => current.Or(x => x.Content == term));
		}

		/// <summary>
		/// Adds any clause.
		/// </summary>
		/// <param name="terms">The value.</param>
		/// <returns></returns>
		protected virtual Expression<Func<T, bool>> AddAnyClause(string[] terms)
		{
			var predicate = PredicateBuilder.False<T>();

			return terms.Aggregate(predicate, (current, term) => current.Or(x => x.Content == term));
		}

		protected virtual Expression<Func<T, bool>> AddAllClause(string[] value)
		{
			string query = value.FirstOrDefault();

			if (string.IsNullOrEmpty(query)) return null;

			// Necessary to surround with quotes for promximity logic to work.
			// (term1 term2~10000) doesn't work, but ("term1 term2"~100000) does.
			var quoted_q = string.Format("\"{0}\"", query);

			return item => item.Content.Like(quoted_q, 1);
		}
	}
}
