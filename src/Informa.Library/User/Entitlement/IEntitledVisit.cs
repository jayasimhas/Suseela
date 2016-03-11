using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledVisit
    {
        IList<IEntitlement> Entitlements { get; } 
    }
}