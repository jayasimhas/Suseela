using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public class SalesforceEntitlmentFactory : ISalesforceEntitlmentFactory
	{
		public SalesforceEntitlement Create(IN_Entitlement entitlement)
		{
			var archiveLimitedDays = ArchiveCodeToDays(entitlement.ArchiveCode);
			var archiveLimited = archiveLimitedDays != -1;

			return new SalesforceEntitlement
			{
				ArchiveCode = entitlement.ArchiveCode,
				ArchiveLimited = archiveLimited,
				ArchiveLimitedDays = archiveLimitedDays,
				DocumentId = entitlement.documentId,
				ProductCode = entitlement.ProductCode,
				ProductId = entitlement.productGUID,
				ProductType = entitlement.ProductType,
				OpportunityId = entitlement.opportunityId,
				OpportunityLineItemId = entitlement.opportunityLineItemId
			};
		}

		public int ArchiveCodeToDays(string archiveCode)
		{
			switch(archiveCode.ToLower())
			{
				case "full":
				case "n/a":
					return -1;
				case "555":
					return 540;
				default:
					var days = 0;
					int.TryParse(archiveCode, out days);

					return days;
			}
		}
	}
}
