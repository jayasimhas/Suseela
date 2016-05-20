﻿using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class EntitlementAccessContext : IEntitlementAccessContext
	{
		protected readonly IEntitlementChecksEnabled EntitlementChecksEnabled;
		protected readonly IEntitlementsContexts EntitlementsContexts;

		public EntitlementAccessContext(
			IEntitlementChecksEnabled entitlementChecksEnabled,
			IEntitlementsContexts entitlementsContexts)
		{
			EntitlementChecksEnabled = entitlementChecksEnabled;
			EntitlementsContexts = entitlementsContexts;
		}

		public IEntitlementAccess Create(string productCode)
		{
			if (!EntitlementChecksEnabled.Enabled)
			{
				return Create(null, EntitledAccessLevel.Individual);
			}

			var entitlements = EntitlementsContexts.SelectMany(ec => ec.Entitlements.Where(e => string.Equals(e.ProductCode, productCode, StringComparison.InvariantCultureIgnoreCase)).Select(e => Create(e, ec.AccessLevel)));

			foreach (var accessLevel in OrderedAccessLevels)
			{
				var entitlementAccess = entitlements.FirstOrDefault(e => e.AccessLevel == accessLevel);

				if (entitlementAccess != null)
				{
					return entitlementAccess;
				}
			}

			return Create(null, EntitledAccessLevel.None);
		}

		public EntitlementAccess Create(IEntitlement entitlement, EntitledAccessLevel accessLevel)
		{
			return new EntitlementAccess
			{
				AccessLevel = accessLevel,
				Entitlement = entitlement
			};
		}

		public List<EntitledAccessLevel> OrderedAccessLevels => new List<EntitledAccessLevel>
		{
			{ EntitledAccessLevel.Individual },
			{ EntitledAccessLevel.TransparentIP },
			{ EntitledAccessLevel.Site }
		};
	}
}
