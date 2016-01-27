using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.SearchTypes;

namespace Velir.Search.Core.Search.Sorts
{
	public abstract class AbstractSortBuilder<T> : ISortBuilder<T> where T : SearchResultItem
	{
		protected IEnumerable<SortOption> SortOptions { get; set; }

		protected AbstractSortBuilder(IEnumerable<SortOption> sortOptions)
		{
			SortOptions = sortOptions != null ? sortOptions.Where(so => so != null) : new List<SortOption>();
		} 

		public Func<IQueryable<T>, IOrderedQueryable<T>> Build()
		{
			if (SortOptions != null && SortOptions.Any())
			{
				return q =>
				{
					IOrderedQueryable<T> orderedQuery = null;

					foreach (var sortOption in SortOptions)
					{
						Expression<Func<T, IComparable>> expression = GenerateMemberExpression<IComparable>(sortOption.FieldName);

						if (expression != null)
						{
							if (orderedQuery == null)
							{
								orderedQuery = sortOption.SortDirection == SortDirection.Ascending ? q.OrderBy(expression) : q.OrderByDescending(expression);
							}
							else
							{
								orderedQuery = sortOption.SortDirection == SortDirection.Ascending ? orderedQuery.ThenBy(expression) : orderedQuery.ThenByDescending(expression);
							}
						}
					}

					return orderedQuery;
				};
			}

			return null;
		}

		private static Expression<Func<T, TK>> GenerateMemberExpression<TK>(string propertyName)
		{
			var propertyInfo = typeof(T).GetProperty(propertyName);

			if (propertyInfo == null) return null;

			var entityParam = Expression.Parameter(typeof(T), "e");

			Expression columnExpr = Expression.Property(entityParam, propertyInfo);

			if (propertyInfo.PropertyType != typeof (TK))
			{
				columnExpr = Expression.Convert(columnExpr, typeof(TK));
			}
				
			return Expression.Lambda<Func<T, TK>>(columnExpr, entityParam);
		}
	}
}
