using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IAuthenticatedIPContext
    {
        bool IsEntitled(IEntitlement entitlement);
        IEnumerable<IEntitlement> Entitlements { get; }
    }
}