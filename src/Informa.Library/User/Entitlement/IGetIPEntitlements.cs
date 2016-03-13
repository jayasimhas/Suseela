using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IGetIPEntitlements
    {
        IList<IEntitlement> GetEntitlements(string ipaddress);
    }
}