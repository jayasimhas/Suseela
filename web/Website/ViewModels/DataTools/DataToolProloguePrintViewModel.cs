using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.DataTools
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class DataToolProloguePrintViewModel : IDataToolProloguePrintViewModel
    {
        private readonly IAuthenticatedUserContext _authenticatedUserContext;
        public DataToolProloguePrintViewModel(IAuthenticatedUserContext authenticatedUserContext)
        {
            _authenticatedUserContext = authenticatedUserContext;
        }

        public bool IsUserAuthenticated => _authenticatedUserContext.IsAuthenticated;

    }
}