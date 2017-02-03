using System;
using System.Linq;
using Informa.Library.Article.Search;
using Sitecore.ContentSearch.Linq;
using Informa.Library.Search.Results;
using Sitecore.ContentSearch.Linq.Utilities;
using Informa.Library.Search.Filter;
using Sitecore.ContentSearch.SearchTypes;
using Informa.Library.Utilities.References;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.Search.Extensions
{

    public static class QueryableExtensions
    {
        public static IQueryable<T> FilterTaxonomies<T>(this IQueryable<T> source, ITaxonomySearchFilter filter, IItemReferences refs, IGlobalSitecoreService service, IVerticalRootContext verticalRootContext)
                        where T : ITaxonomySearchResults
        {
            if (source == null || filter == null || !filter.TaxonomyIds.Any())
                return source;

            var taxItems = filter.TaxonomyIds.Select(a => service.GetItem<ITaxonomy_Item>(a));
            if (taxItems == null || !taxItems.Any())
                return source;

            //breaking up the taxonomies by their respective folder to 'or' any within a folder and 'and' the folders together
            List<Guid> taxGuids = null;
            if (verticalRootContext != null && verticalRootContext?.Item != null && !string.IsNullOrEmpty(verticalRootContext?.Item?.Vertical_Name))
            {


                if (string.Equals(verticalRootContext.Item.Vertical_Name, "Pharma", StringComparison.OrdinalIgnoreCase))
                {
                    taxGuids = new List<Guid>()
                    {
                    refs.RegionsTaxonomyFolder,
                    refs.ContentTypesFolder,
                    refs.MediaTypeIconsFolder,
                    refs.PharmaSubjectsTaxonomyFolder,
                    refs.PharmaTherapyAreasTaxonomyFolder,
                    refs.PharmaDeviceAreasTaxonomyFolder,
                    refs.PharmaIndustriesTaxonomyFolder,
                    refs.PharmaTaxonomyRootFolder
                    };
                }
                else if (string.Equals(verticalRootContext.Item.Vertical_Name, "Agri", StringComparison.OrdinalIgnoreCase))
                {
                    taxGuids = new List<Guid>()
                    {
                    refs.RegionsTaxonomyFolder,
                    refs.ContentTypesFolder,
                    refs.MediaTypeIconsFolder,
                    refs.AgriTaxonomyRootFolder,
                    refs.AgriAgencyRegulatorTaxonomyFolder,
                    refs.AgriAnimalHealthTaxonomyFolder,
                    refs.AgriCommercialTaxonomyFolder,
                    refs.AgriCommodityTaxonomyFolder,
                    refs.AgriCompaniesTaxonomyFolder,
                    refs.AgriCropProtectionaxonomyFolder,
                    refs.AgriIndustriesTaxonomyFolder
                    };
                }
                else if (string.Equals(verticalRootContext.Item.Vertical_Name, "Maritime", StringComparison.OrdinalIgnoreCase))
                {
                    taxGuids = new List<Guid>()
                    {
                    refs.RegionsTaxonomyFolder,
                    refs.ContentTypesFolder,
                    refs.MediaTypeIconsFolder,
                    refs.MaritimeTaxonomyRootFolder,
                    refs.MaritimeCompaniesTaxonomyFolder,
                    refs.MaritimeHotTopicsTaxonomyFolder,
                    refs.MaritimeMarketsTaxonomyFolder,
                    refs.MaritimeRegularsTaxonomyFolder,
                    refs.MaritimeSectorsTaxonomyFolder,
                    refs.MaritimeTopicTaxonomyFolder
                    };
                }
                else
                {
                    taxGuids = new List<Guid>();
                }
            }

            var predicate = PredicateBuilder.True<T>();
            taxGuids
                .Select(g => GetPredicate<T>(service, taxItems, g))
                .Where(p => p != null)
                .ToList()
                .ForEach(i => predicate = predicate.And(i));

            return source.Filter(predicate);
        }

        private static Expression<Func<T, bool>> GetPredicate<T>(IGlobalSitecoreService service, IEnumerable<ITaxonomy_Item> allItems, Guid folder) where T : ITaxonomySearchResults
        {

            var predicate = PredicateBuilder.False<T>();

            var folderItem = service.GetItem<IFolder>(folder);
            if (folderItem == null)
                return null;            

            var list = allItems.Where(b => b._Path.StartsWith(folderItem._Path));
            if (list == null || !list.Any())
                return null;

            foreach (ITaxonomy_Item t in list)
            {
                var children = t._ChildrenWithInferType.OfType<ITaxonomy_Item>();
                if (children != null && children.Any())
                    list = list.Concat(children);
            }

            foreach (var l in list)
            {
                predicate = predicate.Or(p => p.Taxonomies.Contains(l._Id));
            }
            return predicate;
        }

        public static IQueryable<T> FilterContentType<T>(this IQueryable<T> source, ITaxonomySearchFilter filter)
               where T : ITaxonomySearchResults
        {
            if (source == null || filter == null || string.IsNullOrEmpty(filter.ContentTypeTaxonomyId.ToString()) || filter.ContentTypeTaxonomyId == Guid.Empty)
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();
            predicate = predicate.Or(i => i.ContentTypeTaxonomy == filter.ContentTypeTaxonomyId);

            return source.Filter(predicate);
        }

        public static IQueryable<T> FilterByMediaType<T>(this IQueryable<T> source, ITaxonomySearchFilter filter)
                   where T : ITaxonomySearchResults
        {
            if (source == null || filter == null || string.IsNullOrEmpty(filter.MediaTypeTaxonomyId.ToString()) || filter.MediaTypeTaxonomyId == Guid.Empty)
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();
            predicate = predicate.Or(i => i.MediaTypeTaxonomy == filter.MediaTypeTaxonomyId);

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
            if (filter.AuthorGuids.Any())
            {
                var predicate = PredicateBuilder.False<T>();
                predicate = filter.AuthorGuids.Aggregate(predicate, (current, f) => current.Or(i => i.AuthorGuid.Contains(f)));
                return source.Filter(predicate);
            }
            else if (filter.AuthorFullNames.Any())
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

        public static IQueryable<T> FilteryByLegacyArticleNumber<T>(this IQueryable<T> source, ILegacyArticleNumberFilter filter)
                        where T : ILegacyArticleNumber
        {
            if (source == null || filter == null || string.IsNullOrEmpty(filter.LegacyArticleNumber))
            {
                return source;
            }

            var predicate = PredicateBuilder.True<T>();
            predicate = predicate.Or(i => i.LegacyArticleNumber == filter.LegacyArticleNumber);

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
    }
}
