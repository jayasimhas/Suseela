using System.Collections.Generic;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class AuthenticatedIpContext : IAuthenticatedIPContext
    {
        #region Implementation of IAuthenticatedIPContext

        public bool IsEntitled(IEntitlement entitlement)
        {
            return false;
        }

        public IEnumerable<IEntitlement> Entitlements => new List<IEntitlement>();

        #endregion
    }
}