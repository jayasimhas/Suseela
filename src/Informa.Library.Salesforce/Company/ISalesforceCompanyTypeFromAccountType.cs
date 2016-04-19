using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public interface ISalesforceCompanyTypeFromAccountType
	{
		CompanyType Parse(string accountType);
	}
}
