using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public interface ISalesforceCompanyFromSiteType
	{
		CompanyType Parse(string siteType);
	}
}
