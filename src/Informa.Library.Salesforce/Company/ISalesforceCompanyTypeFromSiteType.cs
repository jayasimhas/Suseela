using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public interface ISalesforceCompanyTypeFromSiteType
	{
		CompanyType Parse(string siteType);
	}
}
