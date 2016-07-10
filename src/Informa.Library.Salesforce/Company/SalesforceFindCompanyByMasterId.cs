using Informa.Library.Company;
using Informa.Library.Salesforce.EBIWebServices;
using System;
using System.Linq;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceFindCompanyByMasterId : IFindCompanyByMasterId
	{
		protected readonly ISalesforceCompanyTypeFromAccountType CompanyTypeParser;
		protected readonly ISalesforceServiceContext Service;

		public SalesforceFindCompanyByMasterId(
			ISalesforceCompanyTypeFromAccountType companyTypeParser,
			ISalesforceServiceContext service)
		{
			CompanyTypeParser = companyTypeParser;
			Service = service;
		}

		public IMasterCompany Find(string masterid, string masterPassword)
		{
			var response = Service.Execute(s => s.queryAccountByMasterId(masterid, masterPassword));

			if (!response.IsSuccess())
			{
				if (response.errors.Any(e => string.Equals(e.statusCode, "EXPIRED_MASTERID", StringComparison.InvariantCultureIgnoreCase)))
				{
					return new SalesforceMasterCompany
					{
						IsExpired = true
					};
				}

				return null;
			}

			var account = response.account;

			return new SalesforceMasterCompany
			{
				Id = account?.accountId,
				Name = account?.company,
				Type = CompanyTypeParser.Parse(account?.accountType),
				IsExpired = response.isMasterIdExpiredSpecified && response.isMasterIdExpired != null ? response.isMasterIdExpired.Value : false
			};
		}
	}
}
