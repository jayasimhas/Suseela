using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User
{
    public class SalesforceGetUserEntitlements : ISalesforceGetUserEntitlements, IGetUserEntitlements
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceGetUserEntitlements(ISalesforceServiceContext service)
        {
            Service = service;
        }

        public IList<IEntitlement> GetEntitlements(string email, string ipaddress)
        {
            var response = Service.Execute(x => x.queryEntitlements(email, ipaddress));

            if (!response.IsSuccess())
            {
                return new List<IEntitlement>();
            }
            return new List<IEntitlement>(
                response.entitlements?.Select(x => new Entitlement {ProductCode = x.ProductCode}) ??
                new List<Entitlement>());
        }
    }
}