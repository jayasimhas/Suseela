using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.PXM.Helpers
{
    public interface IInjectAdditionalFields
    {

    }

    [AutowireService]
    public class InjectAdditionalFields : IInjectAdditionalFields
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {

        }

        public InjectAdditionalFields(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string GetAuthors()
        {
            return null;
        }
    }
}