using Informa.Library.Company;

namespace Informa.Library.Salesforce.Company
{
	public class SalesforceMasterCompany : SalesforceCompany, IMasterCompany
	{
		public bool IsExpired { get; set; }
	}
}
