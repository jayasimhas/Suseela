namespace Informa.Library.User.Entitlement
{
    public interface IEntitlement
    {
		string ProductCode { get; set; }
		string ArchiveCode { get; set; }
		string DocumentId { get; set; }
		string ProductId { get; set; }
		string ProductType { get; set; }
	}
}