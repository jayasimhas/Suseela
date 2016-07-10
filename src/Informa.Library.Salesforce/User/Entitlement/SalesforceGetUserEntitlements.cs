using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User.Entitlement
{
    public class SalesforceGetUserEntitlements : ISalesforceGetUserEntitlements, IGetUserEntitlements
    {
		protected readonly ISalesforceEntitlmentFactory EntitlementFactory;
		protected readonly ISalesforceServiceContext Service;

        public SalesforceGetUserEntitlements(
			ISalesforceEntitlmentFactory entitlementFactory,
			ISalesforceServiceContext service)
        {
			EntitlementFactory = entitlementFactory;
			Service = service;
        }

        public IList<IEntitlement> GetEntitlements(string email, string ipaddress)
        {
            if(string.IsNullOrEmpty(email))
                return Enumerable.Empty<IEntitlement>().ToList();

            var response = Service.Execute(x => x.INqueryEntitlements(email, ipaddress));

			if (!response.IsSuccess() || response.entitlements == null)
			{
				return Enumerable.Empty<IEntitlement>().ToList();
			}

			return response.entitlements?.Select(x => EntitlementFactory.Create(x) as IEntitlement).ToList();
		}
    }
}