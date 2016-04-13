using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
    public abstract class EntitledViewModel<T> : GlassViewModel<T> where T : class, IEntitledProductItem, IGlassBase
    {   
        public readonly IEntitledProductEntitlementAccessLevelContext EntitledProductContext;

        public EntitledAccessLevel AccessLevel => EntitledProductContext.GetAccessLevel(GlassModel);

        protected EntitledViewModel(IEntitledProductEntitlementAccessLevelContext entitledProductContext)
        {
            EntitledProductContext = entitledProductContext;
        }

        public virtual bool IsFree => false;

        public bool IsEntitled()
        {
            return IsFree || AccessLevel != EntitledAccessLevel.UnEntitled;
        }
    }
}