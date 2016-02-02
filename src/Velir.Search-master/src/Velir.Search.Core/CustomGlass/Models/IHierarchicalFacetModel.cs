using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jabberwocky.Glass.Factory.Attributes;
using Jabberwocky.Glass.Factory.Interfaces;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Search.Results.Facets;
using Velir.Search.Models;

namespace Velir.Search.Core.CustomGlass.Models
{
	[GlassFactoryType(typeof(IHierarchical_Facet))]
	public abstract class IHierarchicalFacetModel : BaseInterface<IHierarchical_Facet>, ISearchRefinement
	{
		protected IHierarchicalFacetModel(IHierarchical_Facet innerItem) : base(innerItem)
		{
		}

		public abstract string RefinementLabel { get; }
		public abstract string RefinementKey { get; }
		public abstract string FieldName { get; }
		public abstract Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem;
		public abstract bool FacetOnMe { get; }

		public abstract Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem;

		public IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues)
		{
			var facetDict = new Dictionary<string, FacetResultValue>();
			foreach (var value in allValues)
			{
				facetDict[value.Name] = value;
			}

			if (selectedValues != null)
			{
				foreach (var selectedValue in selectedValues.Where(selectedValue => facetDict.ContainsKey(selectedValue)))
				{
					facetDict[selectedValue].Selected = true;
				}
			}
			
			var orderedResults = new List<FacetResultValue>();
			
			if (InnerItem.Root_Item == null) return orderedResults;
			
			foreach (var child in InnerItem.Root_Item._ChildrenWithInferType)
			{
				if (!InnerItem.Valid_Templates.Contains(child._TemplateId)) continue;
				if (!facetDict.ContainsKey(child._Name)) continue;

				var firstLevelValue = facetDict[child._Name];

				firstLevelValue.Sublist =
					child._ChildrenWithInferType.Where(c => InnerItem.Valid_Templates.Contains(c._TemplateId))
						.Where(c => facetDict.ContainsKey(c._Name))
						.Select(c => facetDict[c._Name]).Where(f => f != null);

				orderedResults.Add(firstLevelValue);
			}

			return orderedResults;
		}
	}
}
