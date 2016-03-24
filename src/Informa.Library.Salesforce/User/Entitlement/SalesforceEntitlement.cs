using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public class SalesforceEntitlement : IEntitlement
	{
		public string ProductCode { get; set; }
		public string ArchiveCode { get; set; }
		public string DocumentId { get; set; }
		public string ProductId { get; set; }
		public string ProductType { get; set; }
	}
}
