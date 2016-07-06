using Informa.Library.Authors;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.ViewModels.Authors
{
    public class AuthorProfileViewModel
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            IAuthorIndexClient AuthorIndexClient { get; set; }
        }
        public AuthorProfileViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string Output => _dependencies.AuthorIndexClient.GetAuthorIdByUrlName("moose").ToString();
    }
}