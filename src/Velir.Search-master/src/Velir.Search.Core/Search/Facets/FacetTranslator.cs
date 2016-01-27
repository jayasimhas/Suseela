using System;
using System.Collections.Generic;
using System.Web.OData.Query;
using Jabberwocky.Glass.Factory;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Search.Results.Facets;
using Velir.Search.Models;

namespace Velir.Search.Core.Search.Facets
{
	public class FacetTranslator : IFacetTranslator
	{
		protected IDictionary<string, Tuple<ISearchRefinement, string[]>> RefinementLookup { get; set; }

		public FacetTranslator(IGlassInterfaceFactory factory, I_Refinement_Configuration config, IDictionary<string, string> selectedFacets)
		{
			RefinementLookup = new Dictionary<string, Tuple<ISearchRefinement, string[]>>();

			var availableRefinements = new List<I_Refinement>();
			if (config != null)
			{
				if (config.Refinements_To_Displays != null)
				{
					availableRefinements.AddRange(config.Refinements_To_Displays);
				}

				if (config.Hidden_Available_Refinements != null)
				{
					availableRefinements.AddRange(config.Hidden_Available_Refinements);
				}
			}

			IEnumerable<ISearchRefinement> refinements = factory.GetItems<ISearchRefinement>(availableRefinements);
			if (refinements != null)
			{
				foreach (ISearchRefinement refinement in refinements)
				{
					RefinementLookup[refinement.FieldName] = new Tuple<ISearchRefinement, string[]>(refinement, selectedFacets.ContainsKey(refinement.RefinementKey) ? selectedFacets[refinement.RefinementKey].Split(';') : null);
				}
			}
		}

		public string GetFacetKey(string fieldName)
		{
			var tuple = GetRefinementTuple(fieldName);

			return tuple != null ? tuple.Item1.RefinementKey : null;
		}

		public IEnumerable<FacetResultValue> SortValues(string fieldName, IEnumerable<FacetResultValue> values)
		{
			var tuple = GetRefinementTuple(fieldName);
			var refinement = tuple != null ? tuple.Item1 : null;
			var selectedValues = tuple != null ? tuple.Item2 : null;
			if (refinement != null)
			{
				return refinement.GetFacetOrder(values, selectedValues);
			}

			return values;
		}

		private string[] FormattedFieldNames(string fieldName)
		{
			return new[] { string.Format("{0}_sm", fieldName), string.Format("{0}_t", fieldName), string.Format("{0}_s", fieldName) };
		}

		private Tuple<ISearchRefinement, string[]> GetRefinementTuple(string fieldName)
		{
			var formattedNames = FormattedFieldNames(fieldName);

			foreach (string key in formattedNames)
			{
				if (RefinementLookup.ContainsKey(key))
				{
					return RefinementLookup[key];
				}
			}

			return null;
		}
	}
}
