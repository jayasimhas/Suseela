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
	[GlassFactoryType(typeof(I_Refinement))]
	public abstract class IRefinementModel : BaseInterface<I_Refinement>, ISearchRefinement
	{
		protected IRefinementModel(I_Refinement innerItem) : base(innerItem)
		{
		}

		public string RefinementLabel
		{
			get { return InnerItem.Filter_By_Label; }
		}

		public string RefinementKey { get { return InnerItem.Key; } }
		public string FieldName { get { return InnerItem.Field_Name; } }

		public abstract Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem;
		
		public bool FacetOnMe { get { return false; } }

		public Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem
		{
			throw new InvalidOperationException(string.Format("{0} is not a valid field to facet on.", InnerItem.Field_Name));
		}

		public IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues)
		{
			if (allValues == null) return new List<FacetResultValue>();

			return allValues.Select(v => new FacetResultValue(v.Name, v.Count, selectedValues != null && selectedValues.Contains(v.Name)));
		}
	}
}
