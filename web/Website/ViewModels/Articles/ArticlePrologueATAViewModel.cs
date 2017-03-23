using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class ArticlePrologueATAViewModel : IArticlePrologueATAViewModel
    {
        private readonly IAuthenticatedUserContext _authenticatedUserContext;

        public ArticlePrologueATAViewModel(IAuthenticatedUserContext authenticatedUserContext)
        {
            _authenticatedUserContext = authenticatedUserContext;
        }

        public bool IsUserAuthenticated => _authenticatedUserContext.IsAuthenticated;
    }
}