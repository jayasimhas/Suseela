using Informa.Library.User.Entitlement;

namespace Informa.Library.Salesforce.User.Entitlement
{
	public class SalesforceEntitlement : IEntitlement
	{
		public string ProductCode { get; set; }
		public string ArchiveCode { get; set; }
		public int ArchiveLimitedDays { get; set; }
		public bool ArchiveLimited { get; set; }
		public string DocumentId { get; set; }
		public string ProductId { get; set; }
		public string ProductType { get; set; }
		public string OpportunityId { get; set; }
		public string OpportunityLineItemId { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string SalesEndDate { get; set; }
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AccessEndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
