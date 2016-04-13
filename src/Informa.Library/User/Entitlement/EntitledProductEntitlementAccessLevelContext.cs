using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitledProductEntitlementAccessLevelContext : IEntitledProductEntitlementAccessLevelContext
    {
		protected readonly IEntitlementFactory EntitlementFactory;
        protected readonly IEntitlementAccessLevelContext EntitlementAccessLevelContext;

        public EntitledProductEntitlementAccessLevelContext(
			IEntitlementFactory entitlementFactory,
			IEntitlementAccessLevelContext entitlementAccessLevelContext)
        {
			EntitlementFactory = entitlementFactory;
            EntitlementAccessLevelContext = entitlementAccessLevelContext;
		}

        #region Implementation of IEntitledProductContext

        public EntitledAccessLevel GetAccessLevel(IEntitledProductItem productItem)
        {
            if (productItem == null)
			{
				return EntitledAccessLevel.UnEntitled;
			}

			if (productItem.IsFree)
			{
				return EntitledAccessLevel.Free;
			}

			var entitlement = EntitlementFactory.Create(productItem);

			return EntitlementAccessLevelContext.Determine(entitlement);
        }


        #endregion
    }
}