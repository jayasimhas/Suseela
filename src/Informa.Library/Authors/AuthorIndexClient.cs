using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;

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
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }
        public AuthorIndexClient(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public IStaff_Item GetAuthorByUrlName(string urlName)
        {
            var id = GetAuthorIdByUrlName(urlName);
            if(!id.HasValue) { return null; }

            return _dependencies.SitecoreService.GetItem<IStaff_Item>(id.Value);
        }
        public Guid? GetAuthorIdByUrlName(string urlName)
        {
            using (var context = ContentSearchManager.GetIndex(Constants.AuthorsIndexName).CreateSearchContext())
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