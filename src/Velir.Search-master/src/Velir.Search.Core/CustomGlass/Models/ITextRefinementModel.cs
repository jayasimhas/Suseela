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
	[GlassFactoryType(typeof(IText_Refinement))]
	public abstract class ITextRefinementModel : BaseInterface<IText_Refinement>, ISearchRefinement
	{
		protected ITextRefinementModel(IText_Refinement innerItem) : base(innerItem)
		{
		}

		public abstract string RefinementLabel { get; }
		public abstract string RefinementKey { get; }
		public abstract string FieldName { get; }

		public Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem
		{
			string value = values.FirstOrDefault();

			var predicate = PredicateBuilder.False<T>();
			if (!string.IsNullOrEmpty(value))
			{
				string[] fields = InnerItem.Field_Name.Split(',');
				foreach (string field in fields)
				{
					if (InnerItem.Exact_Match)
					{
						predicate = predicate.Or(x => x[field] == value);
					}
					else
					{
						predicate = predicate.Or(x => x[field].Contains(value));	
					}
				}
				
			}

			return predicate;
		}

		public bool FacetOnMe { get { return false; } }

		public Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem
		{
			return null;
		}

		public abstract IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues);
	}
}
