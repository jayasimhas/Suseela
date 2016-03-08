using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User
{
    public interface IGetIPEntitlements
    {
        IList<IEntitlement> GetEntitlements(string ipaddress);
    }
}