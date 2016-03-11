using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public interface ISalesforceSiteTypeFromCompanyType
	{
		string Parse(CompanyType companyType);
	}
}
