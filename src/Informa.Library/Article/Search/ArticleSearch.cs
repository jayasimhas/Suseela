using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.ContentCuration.Search.Extensions;
using Informa.Library.Search.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using Informa.Library.Search;
using Informa.Library.Utilities.References;
using Sitecore.ContentSearch.Linq;
using System.Text;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Core.Caching;
using Constants = Informa.Library.Utilities.References.Constants;
using Velir.Search.Core.Page;
using Sitecore.Data.Items;
using Informa.Library.Site;
using Velir.Search.Models;
using Informa.Library.Search.SearchIndex;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.Article.Search
{
    [AutowireService]
    public class ArticleSearch : IArticleSearch
    {
        protected readonly IProviderSearchContextFactory SearchContextFactory;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly Func<string, ISitecoreService> SitecoreFactory;
        protected readonly IItemReferences ItemReferences;
        protected readonly ICacheProvider CacheProvider;
        protected readonly ISearchPageParser SearchPageParser;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ISearchIndexNameService IndexNameService;

        public ArticleSearch(
            IProviderSearchContextFactory searchContextFactory,
                IGlobalSitecoreService globalService,
                Func<string, ISitecoreService> sitecoreFactory,
                IItemReferences itemReferences,
                ICacheProvider cacheProvider,
                ISearchPageParser searchPageParser,
                ISitecoreContext context,
                ISiteRootContext siteRootContext,
                ISearchIndexNameService indexNameService
            )
        {
            SearchContextFactory = searchContextFactory;
            GlobalService = globalService;
            SitecoreFactory = sitecoreFactory;
            ItemReferences = itemReferences;
            CacheProvider = cacheProvider;
            SearchPageParser = searchPageParser;
            SitecoreContext = context;
            SiteRootContext = siteRootContext;
            IndexNameService = indexNameService;
        }

        public IArticleSearchFilter CreateFilter()
        {
            return new ArticleSearchFilter
            {
                ExcludeManuallyCuratedItems = new List<Guid>(),
                TaxonomyIds = new List<Guid>(),
                ArticleNumbers = new List<string>(),
                PublicationNames = new List<string>(),
                AuthorNames = new List<string>(),
                CompanyRecordNumbers = new List<string>(),
            };
        }

        public IArticleSearchResults Search(IArticleSearchFilter filter)
        {
            using (var context = SearchContextFactory.Create(IndexNameService.GetIndexName()))
            {
                var query = context.GetQueryable<ArticleSearchResultItem>()
                    .Filter(i => i.TemplateId == IArticleConstants.TemplateId)
                        .FilterByPublications(filter)
                        .FilterByAuthor(filter)
                        .FilterByCompany(filter)
                        .FilterTaxonomies(filter)
                    .ExcludeManuallyCurated(filter)
                        .FilteryByArticleNumbers(filter)
                    .FilteryByEScenicID(filter)
                    .FilteryByRelatedId(filter)
                        .ApplyDefaultFilters();

                if (filter.PageSize > 0)
                {
                    query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
                }

                query = query.OrderByDescending(i => i.ActualPublishDate);

                var results = query.GetResults();

                return new ArticleSearchResults
                {
                    Articles = results.Hits.Select(h => GlobalService.GetItem<IArticle>(h.Document.ItemId.Guid))
                };
            }
        }

        /// <summary>
        /// This is a search implementatation where you want to pass the database name along with the filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public IArticleSearchResults SearchCustomDatabase(IArticleSearchFilter filter, string database, Guid publicationGuid = default(Guid))
        {
            using (var context = SearchContextFactory.Create(database, IndexNameService.GetIndexName(publicationGuid)))
            {
                var query = context.GetQueryable<ArticleSearchResultItem>()
                    .Filter(i => i.TemplateId == IArticleConstants.TemplateId)
                    .FilterTaxonomies(filter)
                    .ExcludeManuallyCurated(filter)
                    .FilteryByArticleNumbers(filter)
                    .FilteryByEScenicID(filter)
                    .ApplyDefaultFilters();

                if (filter.PageSize > 0)
                {
                    query = query.Page(filter.Page > 0 ? filter.Page - 1 : 0, filter.PageSize);
                }

                query = query.OrderByDescending(i => i.ActualPublishDate);
                ISitecoreService localSearchContext = SitecoreFactory(database);
                var results = query.GetResults();
                return new ArticleSearchResults
                {
                    Articles = results.Hits.Select(h => localSearchContext.GetItem<IArticle>(h.Document.ItemId.Guid))
                };
            }
        }

        public IArticleSearchResults FreeWithRegistrationArticles(string database)
        {
            using (var context = SearchContextFactory.Create(database, IndexNameService.GetIndexName()))
            {
                var query = context.GetQueryable<ArticleSearchResultItem>()
                    .Where(a => a.TemplateId == IArticleConstants.TemplateId && a.FreeWithRegistration == true)
                    .ApplyDefaultFilters();

                ISitecoreService localSearchContext = SitecoreFactory(database);
                return new ArticleSearchResults
                {
                    Articles = query.OrderByDescending(i => i.ActualPublishDate).GetResults().Hits.Select(h => localSearchContext.GetItem<IArticle>(h.Document.ItemId.Guid))
                };
            }
        }

        public long GetNextArticleNumber(Guid publicationGuid)
        {
            var publicationItem = GlobalService.GetItem<ISite_Root>(publicationGuid);

            using (var context = SearchContextFactory.Create(Constants.MasterDb, publicationItem.Search_Index_Name))
            {
                if (publicationItem != null)
                {
                    var filter = CreateFilter();
                    var query = context.GetQueryable<ArticleSearchResultItem>()
                        .Filter(i => i.TemplateId == IArticleConstants.TemplateId)
                                .Filter(i => i.PublicationTitle == publicationItem.Publication_Name)
                        .FilterTaxonomies(filter)
                        //.Max(x => x.ArticleIntegerNumber);
                        .OrderByDescending(i => i.ArticleIntegerNumber)
                        .Take(1);

                    var results = query.GetResults();

                    return results?.Hits?.FirstOrDefault()?.Document?.ArticleIntegerNumber + 1 ?? 0;
                    //var results = articleSearchResultItem.GetResults();
                    //var articleResults2 = context.GetQueryable<ArticleSearchResultItem>()     Max(x => x.ArticleIntegerNumber);
                    //return results.Hits.FirstOrDefault().Document.ArticleIntegerNumber;
                }
                return 0;
            }
        }

        /// <summary>
        /// This method gets the Publication Prefix which is used in Article Number Generation.
        /// </summary>
        /// <param name="publicationGuid"></param>
        /// <returns></returns>
        public static string GetPublicationPrefix(Guid publicationGuid)
        {
            string value;
            return Constants.PublicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
        }

        public string GetPublicationPrefix(string publicationGuid)
        {
            var publicationItem = GlobalService.GetItem<ISite_Root>(publicationGuid);
            var verticalItemName = publicationItem?._Parent._Name;// GlobalService.GetItem<IVertical_Root>((Guid)publicationItem?._Parent._Id)?._Name; //Vertical Folder//added,21Sep16
            //Example Content.Pharma.Scrip.Prefix
            var publicationPrefix = SitecoreSettingResolver.Instance.ItemSetting[SitecoreSettingResolver.Instance.ContentRootname + "." + verticalItemName + "." + publicationItem._Name + ".Prefix"];

            return publicationPrefix;
        }

        public static string GetCleansedArticleTitle(IArticle a)
        {
            return Sitecore.Data.Items.ItemUtil.ProposeValidItemName(a.Title.Trim()).Replace(" ", "-");
        }

        public static string GetArticleCustomPath(IArticle a)
        {
            string procName = GetCleansedArticleTitle(a);
            return $"/{a.Article_Number}/{procName}";
        }

        public string GetArticleTaxonomies(Guid id, Guid taxonomyParent)
        {
            string cacheKey = $"{nameof(ArticleSearch)}-GetTaxonomy-{id}";
            return CacheProvider.GetFromCache(cacheKey, () => BuildArticleTaxonomies(id, taxonomyParent));
        }

        public string BuildArticleTaxonomies(Guid id, Guid taxonomyParent)
        {
            var article = GlobalService.GetItem<ArticleItem>(id);
            var taxonomyItems = article?.Taxonomies?.Where(x => x._Parent._Id.Equals(taxonomyParent));

            if (taxonomyItems != null)
            {
                StringBuilder str = new StringBuilder();
                foreach (ITaxonomy_Item taxonomyItem in taxonomyItems)
                {
                    if (str.Length > 0)
                        str.Append(",");
                    str.Append($"'{taxonomyItem.Item_Name.Trim()}'");
                }
                return $"[{str}]";
            }

            return string.Empty;
        }

        public IArticleSearchResults GetLegacyArticleUrl(string path)
        {
            path = path.ToLower();
            using (var context = SearchContextFactory.Create(IndexNameService.GetIndexName()))
            {
                var query = context.GetQueryable<ArticleSearchResultItem>()
                    .Filter(i => i.TemplateId == IArticleConstants.TemplateId)
                    .Filter(i => i.LegacyArticleUrl == path);
                var results = query.GetResults();

                return new ArticleSearchResults
                {
                    Articles = results.Hits.Select(i => GlobalService.GetItem<IArticle>(i.Document.ItemId.Guid))
                };
            }
        }
    }
}
