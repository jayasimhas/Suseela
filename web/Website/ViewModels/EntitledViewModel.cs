using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public abstract class EntitledViewModel<T> : GlassViewModel<T> where T : class, IEntitled_Product
    {
        public readonly IIsEntitledProducItemContext IsEntitledProductItemContext;
        public readonly IAuthenticatedUserContext AuthenticatedUserContext;

        protected EntitledViewModel(
			IIsEntitledProducItemContext isEntitledItemProductContext,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            IsEntitledProductItemContext = isEntitledItemProductContext;
            AuthenticatedUserContext = authenticatedUserContext;
        }

        public virtual bool IsFree => GlassModel.Free;
        public virtual bool IsFreeWithRegistration => GlassModel.Free_With_Registration;

        public bool IsEntitled()
        {
			return IsFree || (IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated) || IsEntitledProductItemContext.IsEntitled(GlassModel);
        }
    }
}