namespace Informa.Library.User.Entitlement
{
    public interface IEntitlement
    {
		string ProductCode { get; }
		string ArchiveCode { get; }
		int ArchiveLimitedDays { get; }
		bool ArchiveLimited { get; }
		string DocumentId { get; }
		string ProductId { get; }
		string ProductType { get; }
		string OpportunityId { get; }
		string OpportunityLineItemId { get; }
        string Type { get; set; }
        string StartDate { get; set; }
        string SalesEndDate { get; set; }
        string ProductName { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string AccessEndDate { get; set; }
    }
}