using Informa.Library.Subscription;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class EntitlementAccessLevelContext : IEntitlementAccessLevelContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;
		protected readonly IEntitlementChecksEnabled EntitlementChecksEnabled;
		protected readonly IUserSubscriptionContext UserSubscriptionContext;
		protected readonly IAuthenticatedIPContext AuthenticatedIPContext;

		public EntitlementAccessLevelContext(
			ISitecoreUserContext sitecoreUserContext,
			IEntitlementChecksEnabled entitlementChecksEnabled,
			IUserSubscriptionContext userSubscriptionContext,
			IAuthenticatedIPContext authenticatedIpContext)
		{
			SitecoreUserContext = sitecoreUserContext;
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

			var isEntitled = SitecoreUserContext.User.IsAdministrator || SitecoreUserContext.Entitlements.Any(x => x.ProductCode == entitlement.ProductCode); ;

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
