using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public abstract class EntitledViewModel<T> : GlassViewModel<T> where T : class, IEntitled_Product
    {
        public readonly IIsEntitledProducItemContext IsEntitledProductItemContext;

        protected EntitledViewModel(
			IIsEntitledProducItemContext isEntitledItemProductContext)
        {
            IsEntitledProductItemContext = isEntitledItemProductContext;
        }

        public virtual bool IsFree => GlassModel.Free;

        public bool IsEntitled()
        {
			return IsFree || IsEntitledProductItemContext.IsEntitled(GlassModel);
        }
    }
}