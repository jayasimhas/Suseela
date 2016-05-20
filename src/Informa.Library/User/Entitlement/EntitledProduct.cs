using System;

namespace Informa.Library.User.Entitlement
{
	public class EntitledProduct : IEntitledProduct
	{
		public string DocumentId { get; set; }
		public bool IsFree { get; set; }
		public string ProductCode { get; set; }
		public DateTime PublishedOn { get; set; }
	}
}