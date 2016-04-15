using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitledProductContext : IEntitledProductContext
    {
		protected readonly IEntitlementFactory EntitlementFactory;
		protected readonly IEntitledContext EntitledContext;

		public EntitledProductContext(
			IEntitlementFactory entitlementFactory,
			IEntitledContext entitledContext)
        {
			EntitlementFactory = entitlementFactory;
			EntitledContext = entitledContext;
		}

		public bool IsEntitled(IEntitledProductItem productItem)
		{
			if (productItem == null)
			{
				return false;
			}

			if (productItem.IsFree)
			{
				return true;
			}

			var entitlement = EntitlementFactory.Create(productItem);
			var entitled = EntitledContext.IsEntitled(entitlement);

			return entitled;
		}
    }
}