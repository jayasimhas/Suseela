using Informa.Library.User.Authentication;
using Informa.Library.User;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
    public abstract class EntitledViewModel<T> : GlassViewModel<T> where T : class, IEntitled_Product
    {
        public readonly IIsEntitledProducItemContext IsEntitledProductItemContext;
        public readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public readonly ISitecoreUserContext SitecoreUserContext;

        protected EntitledViewModel(
            IIsEntitledProducItemContext isEntitledItemProductContext,
            IAuthenticatedUserContext authenticatedUserContext,
            ISitecoreUserContext sitecoreUserContext)
        {
            IsEntitledProductItemContext = isEntitledItemProductContext;
            AuthenticatedUserContext = authenticatedUserContext;
            SitecoreUserContext = sitecoreUserContext;
        }

        public virtual bool IsFree => GlassModel.Free;
        public virtual bool IsFreeWithRegistration => GlassModel.Free_With_Registration;

        public bool IsEntitled(IArticle articleItem)
        {
            return SitecoreUserContext.User.Domain.Name == "sitecore"
              || IsFree
              || (IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated)
              || IsEntitledProductItemContext.IsEntitled(articleItem);
        }
    }
}