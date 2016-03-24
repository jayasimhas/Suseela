using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public interface ISalesforceEntitlmentFactory
	{
		SalesforceEntitlement Create(EBI_Entitlement entitlement);
	}
}