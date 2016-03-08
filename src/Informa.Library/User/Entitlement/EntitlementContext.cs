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
        private readonly IAuthenticatedIPContext AuthenticatedIPContext;  
        public EntitlementContext(
            ISitecoreUserContext sitecoreUserContext,
            IAuthenticatedIPContext authenticatedIpContext)
        {
            SitecoreUserContext = sitecoreUserContext;
            AuthenticatedIPContext = authenticatedIpContext;
        }

        #region Implementation of IEntitlementContext

        public IList<IEntitlement> Entitlements
        {
            get
            {
                var entitlements =
                    new List<IEntitlement>(SitecoreUserContext.User.Profile.GetCustomProperty(nameof(Entitlement))
                        .Split(',')
                        .Select(x => new Entitlement {ProductCode = x}));

                if(Entitlements.Any())
                    return entitlements;

                return AuthenticatedIPContext.Entitlements.ToList();
            }
        }           

        public bool IsEntitled(IEntitlement entitlement)
        {
            return Entitlements.Any(x => x.ProductCode == entitlement.ProductCode);
        }

        #endregion
    }
}