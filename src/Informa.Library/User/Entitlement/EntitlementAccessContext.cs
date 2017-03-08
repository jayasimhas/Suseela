using Informa.Library.SalesforceConfiguration;
using Jabberwocky.Autofac.Attributes;
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
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public EntitlementAccessContext(
            IEntitlementChecksEnabled entitlementChecksEnabled,
            IEntitlementsContexts entitlementsContexts,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            EntitlementChecksEnabled = entitlementChecksEnabled;
            EntitlementsContexts = entitlementsContexts;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public IEntitlementAccess Find(IEntitledProduct entitledProduct)
        {
            if (!EntitlementChecksEnabled.Enabled)
            {
                return Create(null, EntitledAccessLevel.Individual);
            }

            if (entitledProduct == null)
            {
                return Create(null, EntitledAccessLevel.None);
            }

            var entitlements = EntitlementsContexts.
                SelectMany(ec => ec.Entitlements.Where(e => CheckEntitlement(entitledProduct, e)).
                Select(e => Create(e, ec.AccessLevel)));

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

        private bool CheckEntitlement(IEntitledProduct entitledProduct, IEntitlement entitlement)
        {
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled &&
                (string.IsNullOrWhiteSpace(entitlement.AccessEndDate) ||
                Convert.ToDateTime(entitlement.AccessEndDate) < DateTime.Now))
                return false;

            if (entitledProduct.EntitlementLevel == EntitlementLevel.Channel)
            {
                return entitledProduct.Channels.Contains(entitlement.ProductCode) || (!entitledProduct.Channels.Any() && !entitledProduct.IsFree && !string.IsNullOrEmpty(entitlement.ProductCode));
            }
            else if(entitledProduct.EntitlementLevel == EntitlementLevel.Item)
            {
                return entitledProduct.Channels.Contains(entitlement.ProductCode);
            }
            else if (entitledProduct.EntitlementLevel == EntitlementLevel.Site)
            {
                return string.Equals(entitlement.ProductCode, entitledProduct.ProductCode, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}
