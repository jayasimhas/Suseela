using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceSiteTypeParser : ISalesforceCompanyTypeFromSiteType, ISalesforceSiteTypeFromCompanyType, ISalesforceCompanyTypeFromAccountType
	{

		public string Parse(CompanyType companyType)
		{
			switch(companyType)
			{
				case CompanyType.SiteLicenseIP:
					return "SiteLicenseIP";
				case CompanyType.TransparentIP:
					return "TransparentIP";
				default:
					return null;
			}
		}

		CompanyType ISalesforceCompanyTypeFromSiteType.Parse(string siteType)
		{
			switch (siteType)
			{
				case "TransparentIP":
					return CompanyType.TransparentIP;
				case "SiteLicenseIP":
				case "MasterId Only":
				case "IP or MasterId":
					return CompanyType.SiteLicenseIP;
				default:
					return CompanyType.Unknown;
			}
		}

		CompanyType ISalesforceCompanyTypeFromAccountType.Parse(string accountType)
		{
			switch (accountType)
			{
				case "Multi-User":
					return CompanyType.SiteLicenseIP;
				default:
					return CompanyType.Unknown;
			}
		}
	}
}
