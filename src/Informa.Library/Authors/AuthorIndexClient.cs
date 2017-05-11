using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Informa.Library.Search.SearchIndex;
using Jabberwocky.Core.Caching;

namespace Informa.Library.Authors
{
    public interface IAuthorIndexClient
    {
        IStaff_Item GetAuthorByUrlName(string urlName);
        Guid? GetAuthorIdByUrlName(string urlName);
    }

    [AutowireService]
    public class AuthorIndexClient : IAuthorIndexClient
    {

        protected readonly ICacheProvider CacheProvider;
        private readonly IDependencies _dependencies;
        protected readonly ISearchIndexNameService IndexNameService;

        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }
        public AuthorIndexClient(ICacheProvider cacheProvider, IDependencies dependencies, ISearchIndexNameService indexNameService)
        {
            CacheProvider = cacheProvider;
            _dependencies = dependencies;
            IndexNameService = indexNameService;
        }

        private string CreateCacheKey(string suffix)
        {
            return $"{nameof(AuthorIndexClient)}-{suffix}";
        }

        public IStaff_Item GetAuthorByUrlName(string urlName)
        {
            string cacheKey = CreateCacheKey($"AuthorName-{urlName}");
            return CacheProvider.GetFromCache(cacheKey, () => BuildAuthorName(urlName));          
        }

        public IStaff_Item BuildAuthorName(string urlName)
        {
            var id = GetAuthorIdByUrlName(urlName);
            if (!id.HasValue) { return null; }

            return _dependencies.SitecoreService.GetItem<IStaff_Item>(id.Value);
        }
        public Guid? GetAuthorIdByUrlName(string urlName)
        {
            string indexName = IndexNameService.GetAutherIndexName();
           
            if (urlName.Contains(" ")||urlName.Contains("%20"))
            {
                urlName = urlName.Replace(" ", "-");
                urlName = urlName.Replace("%20", "-");
            }
            using (var context = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                var query = context.GetQueryable<AuthorSearchResult>()
                    .Filter(i => i.IsValid)
                    .Filter(i => i.AuthorUrlName.Equals(urlName.ToLower(), StringComparison.InvariantCultureIgnoreCase));
                var hit = query.GetResults()?.Hits?.FirstOrDefault();
                return hit?.Document?.ItemId?.ToGuid();
            }
        }




        
    }

    public class AuthorSearchResult : Sitecore.ContentSearch.SearchTypes.SearchResultItem
    {
        public string AuthorUrlName { get; set; }
        public bool IsValid { get; set; }
    }
}