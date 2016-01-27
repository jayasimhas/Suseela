using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jabberwocky.Glass.Factory.Attributes;
using Jabberwocky.Glass.Factory.Interfaces;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Rules;
using Velir.Search.Core.Search.Results.Facets;
using Velir.Search.Core.Util;
using Velir.Search.Models;

namespace Velir.Search.Core.CustomGlass.Models
{
	[GlassFactoryType(typeof(IBoolean_Refinement))]
	public abstract class IBooleanRefinementModel : BaseInterface<IBoolean_Refinement>, ISearchRefinement
	{
		protected IBooleanRefinementModel(IBoolean_Refinement innerItem) : base(innerItem)
		{
		}

		public abstract string RefinementLabel { get; }
		public abstract string RefinementKey { get; }
		public abstract string FieldName { get; }

		public Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem
		{
			bool comparison = false;
			bool.TryParse(values.FirstOrDefault(), out comparison);

			if ((!InnerItem.Apply_When_True && comparison) || (!InnerItem.Apply_When_False && !comparison))
			{
				return null;
			}

			ParameterExpression expression = Expression.Parameter(typeof(T), "item");
			ConditionContext context = new ConditionContext(expression);

			var equals = new Func<Expression, Expression, Expression>(Expression.Equal);

			return Expression.Lambda<Func<T, bool>>((Expression)equals(Expression.Property(context.ParameterExpression, typeof(T), SearchResultItemUtil.GetPropertyName<T>(InnerItem.Field_Name)), Expression.Constant(comparison)), new[] { expression });
		}

		public bool FacetOnMe { get { return false; } }
		
		public Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem
		{
			return null;
		}

		public abstract IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues);
	}
}
