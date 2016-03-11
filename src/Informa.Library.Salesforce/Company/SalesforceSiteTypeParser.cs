using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceSiteTypeParser : ISalesforceCompanyFromSiteType
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
	}
}
