using Informa.Library.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Threading;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceRetrieveActiveIpRanges : ThreadSafe<List<EBI_IPRange>>, ISalesforceActiveIpRanges, IFindCompanyByIpAddress
	{
		private DateTime LastRefresh;

		protected readonly ISalesforceServiceContext Service;

		public SalesforceRetrieveActiveIpRanges(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public ICompany Find(IPAddress ipAddress)
		{
			throw new NotImplementedException();
		}

		public List<EBI_IPRange> IpRanges => SafeObject;

		protected override List<EBI_IPRange> UnsafeObject
		{
			get
			{
				var response = Service.Execute(s => s.queryAllActiveIPRanges());

				LastRefresh = DateTime.Now;

				if (response.IsSuccess())
				{
					return response.ipRanges.ToList();
				}

				return Enumerable.Empty<EBI_IPRange>().ToList();
			}
		}

		public override bool IsValid => DateTime.Now.AddHours(-2) > LastRefresh;
	}
}
