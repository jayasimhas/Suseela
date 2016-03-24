namespace Informa.Library.User.Entitlement
{
    public class Entitlement : IEntitlement
    {
		public string ArchiveCode { get; set; }
		public string DocumentId { get; set; }
		public string ProductCode { get; set; }
		public string ProductId { get; set; }
		public string ProductType { get; set; }
	}
}