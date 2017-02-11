using System;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	public class EntitledProduct : IEntitledProduct
	{
		public string DocumentId { get; set; }
		public bool IsFree { get; set; }
        public bool IsFreeWithRegistration { get; set; }
		public string ProductCode { get; set; }
		public DateTime PublishedOn { get; set; }
        public IList<string> Channels { get; set; }
        public EntitlementLevel EntitlementLevel { get; set; }
    }
}