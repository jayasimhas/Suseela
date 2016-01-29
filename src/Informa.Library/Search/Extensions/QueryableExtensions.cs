using System.Linq;
using Sitecore.ContentSearch.Linq;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.Linq.Utilities;
using Informa.Library.Search.Filter;

namespace Informa.Library.Search.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> FilterTaxonomies<T>(this IQueryable<T> source, ITaxonomySearchFilter filter)
			where T : ITaxonomySearchResults
		{
			if (source == null || filter == null || !filter.TaxonomyIds.Any())
			{
				return source;
			}

			var predicate = PredicateBuilder.True<T>();

			predicate = filter.TaxonomyIds.Aggregate(predicate, (current, f) => current.Or(i => i.Taxonomies.Contains(f)));

			return source.Filter(predicate);
		}
	}
}
