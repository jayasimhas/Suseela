using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jabberwocky.Glass.Factory.Attributes;
using Jabberwocky.Glass.Factory.Interfaces;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Search.Results.Facets;
using Velir.Search.Models;

namespace Velir.Search.Core.CustomGlass.Models
{
	[GlassFactoryType(typeof(IFacet))]
	public abstract class IFacetModel : BaseInterface<IFacet>, ISearchRefinement
	{
		protected IFacetModel(IFacet innerItem) : base(innerItem)
		{
		}

		public abstract string RefinementLabel { get; }
		public abstract string RefinementKey { get; }
		public abstract string FieldName { get; }

		public Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem
		{
			IFacet facet = InnerItem;
			if (facet != null)
			{
				if (facet.Is_Multi_Value)
				{
					var predicate = PredicateBuilder.True<T>();

					foreach (string value in values)
					{
						predicate = facet.And_Filter
							? predicate.And(x => x[facet.Field_Name] == value)
							: predicate.Or(x => x[facet.Field_Name] == value);
					}

					return predicate;
				}

				string selectedValue = values.FirstOrDefault();
				if (!string.IsNullOrEmpty(selectedValue))
				{
					return item => item[facet.Field_Name] == selectedValue;
				}
			}

			return null;
		}

		public bool FacetOnMe { get { return true; } }

		public Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem
		{
			return item => item[InnerItem.Field_Name];
		}

		public abstract IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues);
	}
}
