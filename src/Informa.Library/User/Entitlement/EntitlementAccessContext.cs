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
        protected readonly IAuthenticatedUserEntitlementsContext AuthenticatedUserEntitlementsContext;

        public EntitlementAccessContext(
            IEntitlementChecksEnabled entitlementChecksEnabled,
            IEntitlementsContexts entitlementsContexts,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IAuthenticatedUserEntitlementsContext authenticatedUserEntitlementsContext)
        {
            EntitlementChecksEnabled = entitlementChecksEnabled;
            EntitlementsContexts = entitlementsContexts;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            AuthenticatedUserEntitlementsContext = authenticatedUserEntitlementsContext;
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

            IEnumerable<EntitlementAccess> entitlements = null;
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                entitlements = AuthenticatedUserEntitlementsContext.Entitlements.Where(e => CheckEntitlement(entitledProduct, e)).
                Select(e => Create(e, AuthenticatedUserEntitlementsContext.AccessLevel));
            }
            else
            {
                entitlements = EntitlementsContexts.
                SelectMany(ec => ec.Entitlements.Where(e => CheckEntitlement(entitledProduct, e)).
                Select(e => Create(e, ec.AccessLevel)));
            }

            foreach (var accessLevel in OrderedAccessLevels)
            {
                var entitlementAccess = entitlements != null ? entitlements.FirstOrDefault(e => e.AccessLevel == accessLevel) : null;

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
                !entitlement.IsActive ||
                Convert.ToDateTime(entitlement.AccessEndDate) < DateTime.Now))
                return false;

            if (entitledProduct.EntitlementLevel == EntitlementLevel.Channel)
            {
                return entitledProduct.Channels.Contains(entitlement.ProductCode) || (!entitledProduct.Channels.Any() && !entitledProduct.IsFree && !string.IsNullOrEmpty(entitlement.ProductCode));
            }
            else if (entitledProduct.EntitlementLevel == EntitlementLevel.Item)
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
