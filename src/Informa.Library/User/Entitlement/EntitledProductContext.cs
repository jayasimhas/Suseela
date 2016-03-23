using System.Linq;
using Informa.Library.Subscription;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitledProductContext : IEntitledProductContext
    {
		protected readonly IEntitlementFactory EntitlementFactory;
		protected readonly IUserSubscriptionContext UserSubscriptionContext;
        protected readonly IAuthenticatedIPContext AuthenticatedIPContext;
        protected readonly IEntitlementContext EntitlementContext;

        public EntitledProductContext(
			IEntitlementFactory entitlementFactory,
			IUserSubscriptionContext userSubscriptionContext,
            IAuthenticatedIPContext authenticatedIpContext,
            IEntitlementContext entitlementContext)
        {
			EntitlementFactory = entitlementFactory;
            UserSubscriptionContext = userSubscriptionContext;
            AuthenticatedIPContext = authenticatedIpContext;
            EntitlementContext = entitlementContext;
    }

        #region Implementation of IEntitledProductContext

        public EntitledAccessLevel GetAccessLevel(IEntitledProductItem productItem)
        {
            var entitlement = EntitlementFactory.Create(productItem);
            //TODO:  Difference logic for 'EntitledProductItem'

            if (productItem == null)
                return EntitledAccessLevel.UnEntitled;

            if (UserSubscriptionContext.IsSubscribed && EntitlementContext.IsEntitled(entitlement))
                return EntitledAccessLevel.Individual;

            if (EntitlementContext.IsEntitled(entitlement))
                return EntitledAccessLevel.Individual;
            //return EntitledAccessLevel.Corporate;



            if (AuthenticatedIPContext.Entitlements.Any(p => p.ProductCode == entitlement.ProductCode))
                return EntitledAccessLevel.TransparentIP;

            //TODO: Free Trial?

            return EntitledAccessLevel.UnEntitled;
        }


        #endregion
    }
}