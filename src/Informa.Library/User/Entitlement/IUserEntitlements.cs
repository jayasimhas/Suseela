using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IUserEntitlements
    {
        IList<IEntitlement> GetEntitlements(string username, string ipAddress);
    }
}