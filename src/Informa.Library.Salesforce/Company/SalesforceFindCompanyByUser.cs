using Informa.Library.Company;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User;
using System.Linq;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceFindCompanyByUser : ISalesforceFindCompanyByUser, IFindCompanyByUser
	{
		protected readonly ISalesforceSiteTypeFromCompanyType SiteTypeParser;
		protected readonly ISalesforceServiceContext Service;

		public SalesforceFindCompanyByUser(
			ISalesforceSiteTypeFromCompanyType siteTypeParser,
			ISalesforceServiceContext service)
		{
			SiteTypeParser = siteTypeParser;
			Service = service;
		}

		public ICompany Find(IUser user)
		{
			if (string.IsNullOrEmpty(user?.Username))
			{
				return null;
			}

			var response = Service.Execute(s => s.queryAccountByUsername(user.Username));

			if (!response.IsSuccess())
			{
				return null;
			}

			var companyType = CompanyType.SiteLicenseIP;
			var siteType = SiteTypeParser.Parse(companyType);
			var companyAccount = response.accounts.FirstOrDefault(a => string.Equals(a.accountType, siteType));

			if (companyAccount == null)
			{
				return null;
			}

			return new SalesforceCompany
			{
				Id = companyAccount.accountId,
				Name = companyAccount.company,
				Type = companyType
			};
		}
	}
}
