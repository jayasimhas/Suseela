using Informa.Library.Subscription.User;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class EntitlementAccessLevelContext : IEntitlementAccessLevelContext
	{
		protected readonly IUserEntitlementsContext UserEntitlementsContext;
		protected readonly IEntitlementChecksEnabled EntitlementChecksEnabled;
		protected readonly IUserSubscribedContext UserSubscriptionContext;
		protected readonly IAuthenticatedIPContext AuthenticatedIPContext;

		public EntitlementAccessLevelContext(
			IUserEntitlementsContext userEntitlementsContext,
			IEntitlementChecksEnabled entitlementChecksEnabled,
			IUserSubscribedContext userSubscriptionContext,
			IAuthenticatedIPContext authenticatedIpContext)
		{
			UserEntitlementsContext = userEntitlementsContext;
			EntitlementChecksEnabled = entitlementChecksEnabled;
			UserSubscriptionContext = userSubscriptionContext;
			AuthenticatedIPContext = authenticatedIpContext;
		}

		public EntitledAccessLevel Determine(IEntitlement entitlement)
		{
			if (!EntitlementChecksEnabled.Enabled)
			{
				return EntitledAccessLevel.Individual;
			}

			var isEntitled = UserEntitlementsContext.Entitlements.Any(x => x.ProductCode == entitlement.ProductCode);

			if (isEntitled) //UserSubscriptionContext.IsSubscribed
			{
				return EntitledAccessLevel.Individual;
			}
			//return EntitledAccessLevel.Corporate;

			if (AuthenticatedIPContext.Entitlements.Any(p => p.ProductCode == entitlement.ProductCode))
			{
				return EntitledAccessLevel.TransparentIP;
			}

			return EntitledAccessLevel.UnEntitled;
		}
	}
}
