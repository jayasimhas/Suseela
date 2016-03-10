using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitlementContext
    {
        IList<IEntitlement> Entitlements { get; }
        bool IsEntitled(IEntitlement entitlement);
    }
}