using Informa.Library.Subscription.User;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;
using System.Text;

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
	    	if (AuthenticatedIPContext.Entitlements.Any(p => p.ProductCode == entitlement.ProductCode))
			{
				return EntitledAccessLevel.TransparentIP;
			}
			return EntitledAccessLevel.UnEntitled;
		}

		public string GetEntitledProducts()
		{
			var entitlementList = UserEntitlementsContext.Entitlements;
			if (entitlementList != null)
			{
				StringBuilder strEntitledProducts = new StringBuilder();
				var lastEntitlement = entitlementList.LastOrDefault();
				strEntitledProducts.Append("[");
				foreach (var entitlement in entitlementList)
				{
					strEntitledProducts.Append("'");
					strEntitledProducts.Append(entitlement.ProductCode);
					strEntitledProducts.Append("'");
					if (entitlementList.Count() > 1 && !lastEntitlement.Equals(entitlement))
					{
						strEntitledProducts.Append(",");
					}
				}
				strEntitledProducts.Append("]");
				return strEntitledProducts.ToString();
			}
			return string.Empty;
		}

		public string GetEntitledProductStatus()
		{
			var entitlementList = UserEntitlementsContext.Entitlements;
			if (entitlementList != null)
			{
				StringBuilder strEntitledProducts = new StringBuilder();
				var lastEntitlement = entitlementList.LastOrDefault();
				strEntitledProducts.Append("[");
				foreach (var entitlement in entitlementList)
				{
					var entitledStatus = Determine(entitlement);
					strEntitledProducts.Append("'");
					strEntitledProducts.Append(entitledStatus.ToString());
					strEntitledProducts.Append("'");
					if (entitlementList.Count() > 1 && !lastEntitlement.Equals(entitlement))
					{
						strEntitledProducts.Append(",");
					}
				}
				strEntitledProducts.Append("]");
				return strEntitledProducts.ToString();
			}
			return string.Empty;
		}
	}
}
