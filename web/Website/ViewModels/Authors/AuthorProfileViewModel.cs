using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.ViewModels.Authors
{
    public class AuthorProfileViewModel
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {

        }
        public AuthorProfileViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }


    }
}