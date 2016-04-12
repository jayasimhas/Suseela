using System.Collections.Generic;
using System.Net;

namespace Informa.Library.User.Entitlement
{
    public interface IGetIPEntitlements
    {
		IList<IEntitlement> GetEntitlements(IPAddress ipAddress);
    }
}