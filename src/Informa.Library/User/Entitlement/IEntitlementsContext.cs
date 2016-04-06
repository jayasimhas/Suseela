using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitlementsContext
    {
        IEnumerable<IEntitlement> Entitlements { get; }
        void RefreshEntitlements();
    }
}