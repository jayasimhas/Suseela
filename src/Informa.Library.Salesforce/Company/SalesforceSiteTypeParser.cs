using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceSiteTypeParser : ISalesforceCompanyTypeFromSiteType, ISalesforceSiteTypeFromCompanyType
	{
		public CompanyType Parse(string siteType)
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
	}
}
