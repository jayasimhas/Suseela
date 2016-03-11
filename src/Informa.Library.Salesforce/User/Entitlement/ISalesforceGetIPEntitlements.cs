using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User.Entitlement
{
    public interface ISalesforceGetIPEntitlements
    {
        IList<IEntitlement> GetEntitlements(string ipaddress);
    }
}