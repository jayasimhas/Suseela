using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public interface ISalesforceEntitlmentFactory
	{
		SalesforceEntitlement Create(IN_Entitlement entitlement);
	}
}