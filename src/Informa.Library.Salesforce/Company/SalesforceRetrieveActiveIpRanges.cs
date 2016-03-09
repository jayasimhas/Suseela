using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceRetrieveActiveIpRanges : ISalesforceActiveIpRanges
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceRetrieveActiveIpRanges(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public void Retrieve()
		{
			var response = Service.Execute(s => s.queryAllActiveIPRanges());

			var ipRanges = response.ipRanges;
		}
	}
}
