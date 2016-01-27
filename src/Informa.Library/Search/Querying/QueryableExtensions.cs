using Informa.Library.Search.Models;
using System.Linq;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;

namespace Informa.Library.Search.Querying
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> ExcludeManuallyCurated<T>(this IQueryable<T> source, IManuallyCuratedContent manuallyCuratedContent)
			where T : SearchResultItem
		{
			var predicate = PredicateBuilder.True<T>();

			predicate = manuallyCuratedContent.ManuallyCuratedItems.Aggregate(predicate, (current, f) => current.Or(i => i.ItemId != ID.Parse(f)));

			return source.Filter(predicate);
		}
	}
}
