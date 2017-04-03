using Informa.Library.SalesforceConfiguration;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
    [AutowireService]
    public class UserEntitlementsContext : IUserEntitlementsContext
    {
        protected readonly IEntitlementsContexts EntitlementsContexts;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IAuthenticatedUserEntitlementsContext AuthenticatedUserEntitlementsContext;

        public UserEntitlementsContext(
            IEntitlementsContexts entitlementsContexts,
             IAuthenticatedUserEntitlementsContext authenticatedUserEntitlementsContext,
             ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            EntitlementsContexts = entitlementsContexts;
            AuthenticatedUserEntitlementsContext = authenticatedUserEntitlementsContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public IEnumerable<IEntitlement> Entitlements => SalesforceConfigurationContext.IsNewSalesforceEnabled ?
            AuthenticatedUserEntitlementsContext.Entitlements :
            EntitlementsContexts.SelectMany(ec => ec.Entitlements);

        public void RefreshEntitlements()
        {
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
                AuthenticatedUserEntitlementsContext.RefreshEntitlements();
            else
                EntitlementsContexts.ToList().ForEach(ec => ec.RefreshEntitlements());
        }
    }
}
