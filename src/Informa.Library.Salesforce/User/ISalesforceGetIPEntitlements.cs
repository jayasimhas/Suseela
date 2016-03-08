﻿using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User
{
    public interface ISalesforceGetIPEntitlements
    {
        IList<IEntitlement> GetEntitlements(string ipaddress);
    }
}