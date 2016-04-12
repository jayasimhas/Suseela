using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IGetUserEntitlements
    {
        IList<IEntitlement> GetEntitlements(string username, string ipAddress);
    }
}