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
	[GlassFactoryType(typeof(IYear_Refinement))]
	public abstract class IYearRefinementModel : BaseInterface<IYear_Refinement>, ISearchRefinement
	{
		protected IYearRefinementModel(IYear_Refinement innerItem) : base(innerItem)
		{
		}

		public abstract string RefinementLabel { get; }
		public abstract string RefinementKey { get; }
		public abstract string FieldName { get; }

		public Expression<Func<T, bool>> GetFilterExpression<T>(IEnumerable<string> values) where T : SearchResultItem
		{
			int year = ValidateInput(values.FirstOrDefault());

			// abort if input is no good
			if (year == 0) return null;

			DateTime startTime = new DateTime(year, 1, 1);
			DateTime endTime = new DateTime(year, 12, 31);

			ParameterExpression expression = Expression.Parameter(typeof(T), "item");
			ConditionContext context = new ConditionContext(expression);

			var greaterThan = new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual);
			var lessThan = new Func<Expression, Expression, Expression>(Expression.LessThanOrEqual);

			var dateRangeExpression = Expression.Lambda<Func<T, bool>>(greaterThan(Expression.Property(context.ParameterExpression, typeof(T), SearchResultItemUtil.GetPropertyName<T>(InnerItem.Field_Name)), Expression.Constant(startTime)), new[] { expression });
			dateRangeExpression = Expression.Lambda<Func<T, bool>>(lessThan(Expression.Property(context.ParameterExpression, typeof(T), SearchResultItemUtil.GetPropertyName<T>(InnerItem.Field_Name)), Expression.Constant(endTime)), new[] { expression });

			return dateRangeExpression;
		}

		public bool FacetOnMe { get { return false; } }

		public Expression<Func<T, object>> GetFacetFieldExpression<T>() where T : SearchResultItem
		{
			return null;
		}

		public abstract IEnumerable<FacetResultValue> GetFacetOrder(IEnumerable<FacetResultValue> allValues, IEnumerable<string> selectedValues);

		private int ValidateInput(string input)
		{
			int year = 0;
			if (!string.IsNullOrEmpty(input))
			{
				if(int.TryParse(input, out year))
				{
					return year;
				}

				// maybe a full date was input.
				DateTime date = DateTime.MinValue;
				if (DateTime.TryParse(input, out date))
				{
					return date.Year;
				}
			}

			return year;
		}
	}
}
