using System;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Authors
{
    public interface IAuthorIndexClient
    {
        string GetUrlName([NotNull] IStaff_Item authorItem);
        string GetUrlName(Guid authorId);
        Guid GetAuthorIdByUrlName(string urlName);
        IStaff_Item GetAuthorByUrlName(string urlName);
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


        public string GetUrlName([NotNull] IStaff_Item authorItem)
            => GetUrlName(authorItem._Id);
        public string GetUrlName(Guid authorId)
        {
            return "fake-aakash-shah";
        }

        public Guid GetAuthorIdByUrlName(string urlName)
        {
            return new Guid("{EAE673A9-87BF-4383-A4E5-046362DD3204}");
        }
        public IStaff_Item GetAuthorByUrlName(string urlName)
        {
            return _dependencies.SitecoreService.GetItem<IStaff_Item>(
                GetAuthorIdByUrlName(urlName));
        }
        
    }
}