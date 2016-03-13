using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitlementContext
    {
        bool IsEntitled(IEntitlement entitlement);
    }
}