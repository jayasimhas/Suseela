using Informa.Library.Company;
using Informa.Library.Salesforce.EBIWebServices;

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

		public ICompany Find(string masterid, string masterPassword)
		{
			var response = Service.Execute(s => s.queryAccountByMasterId(masterid, masterPassword));

			if (!response.IsSuccess())
			{
				return null;
			}

			var account = response.account;

			return new SalesforceCompany
			{
				Id = account.accountId,
				Name = account.company,
				Type = CompanyTypeParser.Parse(account.accountType)
			};
		}
	}
}
