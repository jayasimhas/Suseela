using Informa.Library.User;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public abstract class EntitledViewModel<T> : GlassViewModel<T> where T : class, IEntitled_Product
    {
        public readonly IIsEntitledProducItemContext IsEntitledProductItemContext;
        public readonly ISitecoreUserContext SitecoreUserContext;

        protected EntitledViewModel(
            IIsEntitledProducItemContext isEntitledItemProductContext,
            ISitecoreUserContext sitecoreUserContext)
        {
            IsEntitledProductItemContext = isEntitledItemProductContext;
            SitecoreUserContext = sitecoreUserContext;
        }

        public virtual bool IsFree => GlassModel.Free;

        public bool IsEntitled()
        {
            return IsFree || IsEntitledProductItemContext.IsEntitled(GlassModel) || SitecoreUserContext.User.Domain.Name == "sitecore";
        }
    }
}