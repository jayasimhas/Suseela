using Jabberwocky.Autofac.Attributes;
using System;
using System.Linq;
using Informa.Library.User.Authentication;
using Informa.Library.SalesforceConfiguration;
using System.Collections;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    [AutowireService]
    public class EntitledProductContext : IEntitledProductContext
    {
        protected readonly IEntitlementAccessContexts EntitlementAccessContexts;
        protected readonly IEntitlementAccessContext EntitlementAccessContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public EntitledProductContext(
            IEntitlementAccessContexts entitlementAccessContexts,
            IAuthenticatedUserContext authenticatedUserContext,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            IEntitlementAccessContext entitlementAccessContext)
        {
            EntitlementAccessContexts = entitlementAccessContexts;
            AuthenticatedUserContext = authenticatedUserContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            EntitlementAccessContext = entitlementAccessContext;
        }

        public bool IsEntitled(IEntitledProduct entitledProduct)
        {
            if (entitledProduct == null)
            {
                return false;
            }

            if (entitledProduct.IsFree || (entitledProduct.IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated))
            {
                return true;
            }

            IList<IEntitlementAccess> entitlementAccesses = null;

            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                entitlementAccesses = new List<IEntitlementAccess>();
                entitlementAccesses.Add(EntitlementAccessContext.Find(entitledProduct));
            }
            else
            {
                entitlementAccesses = EntitlementAccessContexts.Select(eac => eac.Find(entitledProduct)).ToList();
            }

            var filteredEntitlementAccesses = entitlementAccesses
                   .Where(ea =>
                       ea != null &&
                       ea.AccessLevel != EntitledAccessLevel.None
                       && isValidEntitlement(ea.Entitlement, entitledProduct)
                   )
                   .ToList();
            return filteredEntitlementAccesses.Any();
        }

        public bool IsArchiveLimited(IEntitlement entitlement, IEntitledProduct entitledProduct)
        {
            if (entitlement == null || !entitlement.ArchiveLimited)
            {
                return false;
            }

            var productPublishedOn = entitledProduct.PublishedOn;
            var archiveLimit = DateTime.Now.AddDays(entitlement.ArchiveLimitedDays * -1);

            return archiveLimit >= productPublishedOn;
        }

        private bool isValidEntitlement(IEntitlement entitlement, IEntitledProduct entitledProduct)
        {
            if (entitlement == null || string.IsNullOrWhiteSpace(entitlement.AccessEndDate))
            {
                return false;
            }
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                var accessEndDate = Convert.ToDateTime(entitlement.AccessEndDate);
                return accessEndDate >= DateTime.Now;
            }
            else
            {
                return !IsArchiveLimited(entitlement, entitledProduct);
            }
        }
    }
}