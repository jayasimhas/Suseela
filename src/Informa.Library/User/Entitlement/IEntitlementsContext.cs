using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitlementsContext
    {
        IEnumerable<IEntitlement> Entitlements { get; }
		EntitledAccessLevel GetProductAccessLevel(string productCode);
		void RefreshEntitlements();
    }
}