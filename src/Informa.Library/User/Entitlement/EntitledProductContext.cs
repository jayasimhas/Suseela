using System.Linq;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitledProductContext : IEntitledProductContext
    {
        private readonly IUserSubscriptionContext UserSubscriptionContext;
        private readonly IAuthenticatedIPContext AuthenticatedIPContext;
        private readonly IEntitlementContext EntitlementContext;
        public EntitledProductContext(
            IUserSubscriptionContext userSubscriptionContext,
            IAuthenticatedIPContext authenticatedIpContext,
            IEntitlementContext entitlementContext)
        {                                                         
            UserSubscriptionContext = userSubscriptionContext;
            AuthenticatedIPContext = authenticatedIpContext;
            EntitlementContext = entitlementContext;
    }

        #region Implementation of IEntitledProductContext

        public EntitledAccessLevel GetAccessLevel(IEntitledProductItem productItem)
        {
            var entitlement = new ScripEntitlement(productItem);
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