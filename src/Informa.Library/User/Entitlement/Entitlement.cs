using System;

namespace Informa.Library.User.Entitlement
{
    [Serializable]
    public class Entitlement : IEntitlement
    {
		public string ArchiveCode { get; set; }
		public int ArchiveLimitedDays { get; set; }
		public bool ArchiveLimited { get; set; }
		public string DocumentId { get; set; }
		public string ProductCode { get; set; }
		public string ProductId { get; set; }
		public string ProductType { get; set; }
	}
}