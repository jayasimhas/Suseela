using System.Linq;
using Sitecore.ContentSearch.Linq;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.Linq.Utilities;
using Informa.Library.Search.Filter;
using Sitecore.ContentSearch.SearchTypes;

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

        public static IQueryable<T> FilteryByArticleNumber<T>(this IQueryable<T> source, IArticleNumberFilter filter)
            where T : IArticleNumber
        {
            if (source == null || filter == null || string.IsNullOrEmpty(filter.ArticleNumber))
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();
            predicate = predicate.Or(i => i.ArticleNumber == filter.ArticleNumber);

            return source.Filter(predicate);
        }

        public static IQueryable<T> FilteryByEScenicID<T>(this IQueryable<T> source, IArticleEScenicIDFilter filter)
            where T : IArticleEScenicID
        {
            if (source == null || filter == null || string.IsNullOrEmpty(filter.EScenicID))
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();
            predicate = predicate.Or(i => i.EScenicID == filter.EScenicID);

            return source.Filter(predicate);
        }
    }
}
