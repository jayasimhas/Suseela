using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails.Components;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.Emails
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class FeaturedContentViewModel : GlassViewModel<IFeatured_Content>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {

        }

        public FeaturedContentViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }
    }
}