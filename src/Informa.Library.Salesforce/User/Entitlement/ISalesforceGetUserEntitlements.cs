using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User.Entitlement
{
    public interface ISalesforceGetUserEntitlements
    {
        IList<IEntitlement> GetEntitlements(string email, string ipaddress);
    }
}