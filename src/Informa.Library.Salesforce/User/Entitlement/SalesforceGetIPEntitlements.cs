using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Entitlement;
using System.Net;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public class SalesforceGetIPEntitlements : ISalesforceGetIPEntitlements, IGetIPEntitlements
	{
		protected readonly ISalesforceEntitlmentFactory EntitlementFactory;
		protected readonly ISalesforceServiceContext Service;

		public SalesforceGetIPEntitlements(
			ISalesforceEntitlmentFactory entitlementFactory,
			ISalesforceServiceContext service)
		{
			EntitlementFactory = entitlementFactory;
			Service = service;
		}

		public IList<IEntitlement> GetEntitlements(IPAddress ipAddress)
		{
			return GetEntitlements(ipAddress.ToString());
		}

		public IList<IEntitlement> GetEntitlements(string ipaddress)
        {
            var response = Service.Execute(x => x.INquerySiteEntitlementsIP(ipaddress));

            if (!response.IsSuccess() || response.entitlements == null)
            {
                return Enumerable.Empty<IEntitlement>().ToList();
            }

			return response.entitlements?.Select(x => EntitlementFactory.Create(x) as IEntitlement).ToList();

		}
    }
}