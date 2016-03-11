using Informa.Library.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Threading;
using Informa.Library.Net;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceFindCompanyByIpAddress : ThreadSafe<List<SalesforceCompany>>, ISalesforceFindCompanyByIpAddress, IFindCompanyByIpAddress
	{
		private DateTime LastRefresh;

		protected readonly IIpAddressRangeCheck IpAddressRangeCheck;
		protected readonly ISalesforceCompanyTypeFromSiteType ParseCompanyType;
		protected readonly ISalesforceServiceContext Service;

		public SalesforceFindCompanyByIpAddress(
			IIpAddressRangeCheck ipAddressRangeCheck,
			ISalesforceCompanyTypeFromSiteType parseCompanyType,
			ISalesforceServiceContext service)
		{
			IpAddressRangeCheck = ipAddressRangeCheck;
			ParseCompanyType = parseCompanyType;
			Service = service;
		}

		public ICompany Find(IPAddress ipAddress)
		{
			if (ipAddress == null)
			{
				return null;
			}

			return Companies.FirstOrDefault(c => c.LowerIpAddress != null && c.UpperIpAddress != null && IpAddressRangeCheck.IsInRange(ipAddress, c.LowerIpAddress, c.UpperIpAddress));
		}

		public List<SalesforceCompany> Companies => SafeObject;

		protected override List<SalesforceCompany> UnsafeObject
		{
			get
			{
				var response = Service.Execute(s => s.queryAllActiveIPRanges());

				LastRefresh = DateTime.Now;

				if (response.IsSuccess())
				{
					return response.ipRanges.Select(ir => CreateCompany(ir)).ToList();
				}

				return Enumerable.Empty<SalesforceCompany>().ToList();
			}
		}

		public override bool IsValid => DateTime.Now.AddHours(-2) > LastRefresh;

		public SalesforceCompany CreateCompany(EBI_IPRange ipRange)
		{
			return new SalesforceCompany
			{
				Id = ipRange.account.accountId,
				Name = ipRange.account.company,
				Type = ParseCompanyType.Parse(ipRange.siteType),
				LowerIpAddress = CreateIpAddress(ipRange.beginRange),
				UpperIpAddress = CreateIpAddress(ipRange.endRange)
			};
		}

		public IPAddress CreateIpAddress(string rawIpAddress)
		{
			IPAddress ipAddress;

			if (IPAddress.TryParse(rawIpAddress, out ipAddress))
			{
				return ipAddress;
			}

			return null;
		}
	}
}
