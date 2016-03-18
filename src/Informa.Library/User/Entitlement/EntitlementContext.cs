using System.Collections.Generic;
using System.Linq;                         
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Mvc.Extensions;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitlementContext : IEntitlementContext
    {
        private readonly ISitecoreUserContext SitecoreUserContext;
        //private readonly IAuthenticatedIPContext AuthenticatedIPContext;
        //private readonly ICompanyContext CompanyContext;
        public EntitlementContext(
            ISitecoreUserContext sitecoreUserContext)
        {
            SitecoreUserContext = sitecoreUserContext;
            //AuthenticatedIPContext = authenticatedIpContext;
            //CompanyContext = companyContext;
        }

        #region Implementation of IEntitlementContext

        public bool IsEntitled(IEntitlement entitlement)
        {
            return SitecoreUserContext.User.IsAdministrator || SitecoreUserContext.Entitlements.Any(x => x.ProductCode == entitlement.ProductCode);
        }

        #endregion
    }
}