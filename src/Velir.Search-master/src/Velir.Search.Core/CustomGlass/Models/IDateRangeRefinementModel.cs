using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jabberwocky.Glass.Factory.Attributes;
using Jabberwocky.Glass.Factory.Interfaces;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Velir.Search.Core.Refinements;
using Velir.Search.Core.Rules;
using Velir.Search.Core.Search.Results.Facets;
using Velir.Search.Core.Util;
using Velir.Search.Models;

namespace Velir.Search.Core.CustomGlass.Models
{
	[GlassFactoryType(typeof(IDate_Range_Refinement))]
	public abstract class IDateRangeRefinementModel : BaseInterface<IDate_Range_Refinement>, ISearchRefinement
	{
		protected IDateRangeRefinementModel(IDate_Range_Refinement innerItem) : base(innerItem)
		{
		}

		public abstract string RefinementLabel { get; }
		public abstract string RefinementKey { get; }
		public abstract string FieldName { get; }

		public Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem
		{
			DateTime startDate = DateTime.MinValue;
			DateTime endDate = DateTime.MinValue;

			DateTime.TryParse(values.First(), out startDate);
			DateTime.TryParse(values.Last(), out endDate);

			// lets make sure items are within the day.
			if (endDate > DateTime.MinValue)
			{
				endDate = endDate.AddHours(23);
				endDate = endDate.AddMinutes(59);
				endDate = endDate.AddSeconds(59);
			}

			ParameterExpression expression = Expression.Parameter(typeof(T), "item");
			ConditionContext context = new ConditionContext(expression);

			var greaterThan = new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual);
			var lessThan = new Func<Expression, Expression, Expression>(Expression.LessThanOrEqual);

			var dateRangeExpression = Expression.Lambda<Func<T, bool>>(greaterThan(Expression.Property(context.ParameterExpression, typeof(T), SearchResultItemUtil.GetPropertyName<T>(InnerItem.Field_Name)), Expression.Constant(startDate)), new[] { expression });
			
			if (endDate > DateTime.MinValue)
			{
				dateRangeExpression = dateRangeExpression.And(Expression.Lambda<Func<T, bool>>(lessThan(Expression.Property(context.ParameterExpression, typeof(T), SearchResultItemUtil.GetPropertyName<T>(InnerItem.Field_Name)), Expression.Constant(endDate)), new[] { expression }));	
			}
			
			return dateRangeExpression;
		}

		public bool FacetOnMe { get { return false; } }
		public Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem
		{
			return null;
		}

		public abstract IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues);
	}
}
