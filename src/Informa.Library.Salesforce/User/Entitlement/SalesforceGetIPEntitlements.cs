using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User.Entitlement
{
    public class SalesforceGetIPEntitlements : ISalesforceGetIPEntitlements, IGetIPEntitlements
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceGetIPEntitlements(ISalesforceServiceContext service)
        {
            Service = service;
        }    
        public IList<IEntitlement> GetEntitlements(string ipaddress)
        {
            var response = Service.Execute(x => x.querySiteEntitlementsIP(ipaddress));
            if (!response.IsSuccess())
            {
                return new List<IEntitlement>();
            }
            return new List<IEntitlement>(
                response.entitlements?.Select(x => new Library.User.Entitlement.Entitlement { ProductCode = x.ProductCode }) ??
                new List<Library.User.Entitlement.Entitlement>());
        }
    }
}