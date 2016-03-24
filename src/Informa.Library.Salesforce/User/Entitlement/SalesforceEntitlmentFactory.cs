using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public class SalesforceEntitlmentFactory : ISalesforceEntitlmentFactory
	{
		public SalesforceEntitlement Create(EBI_Entitlement entitlement)
		{
			return new SalesforceEntitlement
			{
				ArchiveCode = entitlement.ArchiveCode,
				DocumentId = entitlement.documentId,
				ProductCode = entitlement.ProductCode,
				ProductId = entitlement.productGUID,
				ProductType = entitlement.ProductType
			};
		}
	}
}
