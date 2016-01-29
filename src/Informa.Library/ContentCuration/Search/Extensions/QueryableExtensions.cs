using System.Linq;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Informa.Library.ContentCuration.Search.Filter;

namespace Informa.Library.ContentCuration.Search.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> ExcludeManuallyCurated<T>(this IQueryable<T> source, IManuallyCuratedContentFilter filter)
			where T : SearchResultItem
		{
			if (!filter.ExcludeManuallyCuratedItems.Any())
			{
				return source;
			}

			var predicate = PredicateBuilder.True<T>();

			predicate = filter.ExcludeManuallyCuratedItems.Aggregate(predicate, (current, f) => current.Or(i => i.ItemId != ID.Parse(f)));

			return source.Filter(predicate);
		}
	}
}
