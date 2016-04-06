using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class EntitlementAccessLevelContext : IEntitlementAccessLevelContext
	{
		protected readonly IEntitlementChecksEnabled EntitlementChecksEnabled;
		protected readonly IEntitlementsContexts EntitlementsContexts;

		public EntitlementAccessLevelContext(
			IEntitlementChecksEnabled entitlementChecksEnabled,
			IEntitlementsContexts entitlementsContexts)
		{
			EntitlementChecksEnabled = entitlementChecksEnabled;
			EntitlementsContexts = entitlementsContexts;
		}

		public EntitledAccessLevel Determine(IEntitlement entitlement)
		{
			if (!EntitlementChecksEnabled.Enabled)
			{
				return EntitledAccessLevel.Individual;
			}

			var accessLevels = EntitlementsContexts.Select(ec => ec.GetProductAccessLevel(entitlement.ProductCode)).Distinct();

			if (accessLevels.Contains(EntitledAccessLevel.Individual))
			{
				return EntitledAccessLevel.Individual;
			}

			if (accessLevels.Contains(EntitledAccessLevel.TransparentIP))
			{
				return EntitledAccessLevel.TransparentIP;
			}

			return EntitledAccessLevel.UnEntitled;
		}
	}
}
