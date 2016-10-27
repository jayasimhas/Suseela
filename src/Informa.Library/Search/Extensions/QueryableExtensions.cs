using System;
using System.Linq;
using Informa.Library.Article.Search;
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

            predicate = filter.TaxonomyIds.Aggregate(predicate, (current, f) => current.And(i => i.Taxonomies.Contains(f)));

            return source.Filter(predicate);
        }

        public static IQueryable<T> FilterPersonalizedTaxonomies<T>(this IQueryable<T> source, ITaxonomySearchFilter filter) where T : ITaxonomySearchResults
        {
            if (source == null || filter == null || !filter.TaxonomyIds.Any())
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();

            predicate = filter.TaxonomyIds.Aggregate(predicate, (current, f) => current.Or(i => i.Taxonomies.Contains(f)));

            return source.Filter(predicate);
        }
        public static IQueryable<T> FilterByPublications<T>(this IQueryable<T> source, IArticlePublicationFilter filter)
                        where T : IArticlePublicationResults
        {
            if (source == null || filter == null || !filter.PublicationNames.Any())
            {
                return source;
            }

            var predicate = PredicateBuilder.False<T>();

            predicate = filter.PublicationNames.Aggregate(predicate, (current, f) => current.Or(i => i.PublicationTitle == f));

            return source.Filter(predicate);
        }

        public static IQueryable<T> FilterByAuthor<T>(this IQueryable<T> source, IArticleAuthorFilter filter)
        where T : IArticleAuthorResults
        {
            if (source == null || filter == null)
            {
                return source;
            }

            // Filter first by author guid if that's provided, otherwise use the author name.
            // EXPLANATION: Informa asked for the ability to facet on author names, but we still need the GUID lookups for the 
            // word plugin, author lists, etc, since author names aren't guaranteed to be unique.
            if (filter != null && filter.AuthorGuids != null && filter.AuthorGuids.Any())
            {
                var predicate = PredicateBuilder.False<T>();
                predicate = filter.AuthorGuids.Aggregate(predicate, (current, f) => current.Or(i => i.AuthorGuid.Contains(f)));
                return source.Filter(predicate);
            }
            else if (filter != null && filter.AuthorFullNames != null && filter.AuthorFullNames.Any())
            {
                var predicate = PredicateBuilder.False<T>();
                predicate = filter.AuthorFullNames.Aggregate(predicate, (current, f) => current.Or(i => i.AuthorFullNames.Contains(f)));
                return source.Filter(predicate);
            }

            return source;
        }

        public static IQueryable<T> FilterByCompany<T>(this IQueryable<T> source, IArticleCompanyFilter filter)
                where T : IArticleCompanyResults
        {
            if (source == null || filter == null || !filter.CompanyRecordNumbers.Any())
            {
                return source;
            }

            var predicate = PredicateBuilder.False<T>();

            predicate = filter.CompanyRecordNumbers.Aggregate(predicate, (current, f) => current.Or(i => i.CompanyRecordIDs.Contains(f)));

            return source.Filter(predicate);
        }

        public static IQueryable<T> FilteryByArticleNumbers<T>(this IQueryable<T> source, IArticleNumbersFilter filter)
                        where T : IArticleNumber
        {
            if (source == null || filter == null || !filter.ArticleNumbers.Any())
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();

            predicate = filter.ArticleNumbers.Aggregate(predicate, (current, f) => current.Or(an => an.ArticleNumber == f));

            return source.Filter(predicate);
        }

        public static IQueryable<T> FilteryByRelatedId<T>(this IQueryable<T> source, IReferencedArticleFilter filter)
        where T : IReferencedArticles
        {
            if (source == null || filter == null || filter.ReferencedArticle.Equals(Guid.Empty))
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();
            predicate = predicate.Or(i => i.ReferencedArticles.Contains(filter.ReferencedArticle));

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

        public static IQueryable<T> ApplyDefaultFilters<T>(this IQueryable<T> source)
                                                                        where T : ArticleSearchResultItem
        {
            return source?
                    .FilteryByLatestVersion()
                    .FilteryByCurrentLanguage();
        }

        public static IQueryable<T> FilteryByLatestVersion<T>(this IQueryable<T> source)
                                        where T : ArticleSearchResultItem
        {
            return source?.Filter(x => x.IsLatestVersion);
        }

        public static IQueryable<T> FilteryByCurrentLanguage<T>(this IQueryable<T> source)
                                        where T : SearchResultItem
        {
            return source?.Filter(x => x.Language == Sitecore.Context.Language.Name);
        }

        public static IQueryable<T> FilteryByCurrentSite<T>(this IQueryable<T> source)
                                        where T : SearchResultItem
        {
            return source?.Filter(x => x.Sites.Contains(Sitecore.Context.Site.Name.ToLowerInvariant()));
        }
    }
}
