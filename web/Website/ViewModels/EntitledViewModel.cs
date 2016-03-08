using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
    public abstract class EntitledViewModel<T> : GlassViewModel<T> where T : class, IEntitledProductItem, IGlassBase
    {   
        protected readonly IEntitledProductContext EntitledProductContext;

        public virtual EntitledAccessLevel AccessLevel
            => EntitledProductContext.GetAccessLevel(GlassModel);

        public EntitledViewModel(IEntitledProductContext entitledProductContext)
        {
            EntitledProductContext = entitledProductContext;
        } 
    }
}