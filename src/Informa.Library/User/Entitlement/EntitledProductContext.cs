using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitledProductContext : IEntitledProductContext
    {
		protected readonly IEntitlementFactory EntitlementFactory;
        protected readonly IEntitlementAccessLevelContext EntitlementAccessLevelContext;

        public EntitledProductContext(
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
				return EntitledAccessLevel.Individual;
			}

			var entitlement = EntitlementFactory.Create(productItem);

			return EntitlementAccessLevelContext.Determine(entitlement);
        }


        #endregion
    }
}