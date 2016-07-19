using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class ArticleProloguePrintViewModel : IArticleProloguePrintViewModel
    {
        private readonly IAuthenticatedUserContext _authenticatedUserContext;
        public ArticleProloguePrintViewModel(IAuthenticatedUserContext authenticatedUserContext)
        {
            _authenticatedUserContext = authenticatedUserContext;
        }

        public bool IsUserAuthenticated => _authenticatedUserContext.IsAuthenticated;

    }
}