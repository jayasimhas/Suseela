using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class ArticlePrologueAskTheAnalystViewModel : IArticlePrologueAskTheAnalystViewModel
    {
        public IAuthenticatedUserContext _authenticatedUserContext;
        public ArticlePrologueAskTheAnalystViewModel(IAuthenticatedUserContext authenticatedUserContext)
        {
            _authenticatedUserContext = authenticatedUserContext;
        }

        public bool IsUserAuthenticated => _authenticatedUserContext.IsAuthenticated;

    }
}