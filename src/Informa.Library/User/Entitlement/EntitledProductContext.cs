using Jabberwocky.Autofac.Attributes;
using System;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class EntitledProductContext : IEntitledProductContext
    {
		protected readonly IEntitlementAccessContext EntitlementAccessContext;

		public EntitledProductContext(
			IEntitlementAccessContext entitlementAccessContext)
        {
			EntitlementAccessContext = entitlementAccessContext;
		}

		public bool IsEntitled(IEntitledProduct entitledProduct)
		{
			if (entitledProduct == null)
			{
				return false;
			}

			if (entitledProduct.IsFree)
			{
				return true;
			}

			var entitlementAccess = EntitlementAccessContext.Create(entitledProduct.ProductCode);

			if (entitlementAccess.Entitlement == null || !entitlementAccess.Entitlement.ArchiveLimited)
			{
				return entitlementAccess.AccessLevel != EntitledAccessLevel.None;
			}

			var productPublishedOn = entitledProduct.PublishedOn;
			var archiveLimit = DateTime.Now.AddDays(entitlementAccess.Entitlement.ArchiveLimitedDays * -1);

			return productPublishedOn >= archiveLimit;
		}
    }
}