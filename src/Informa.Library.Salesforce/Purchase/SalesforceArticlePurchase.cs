using Informa.Library.Purchase;
using System;

namespace Informa.Library.Salesforce.Purchase
{
	public class SalesforceArticlePurchase : IArticlePurchase
	{
		public string Publication { get; set; }
		public string DocumentId { get; set; }
		public DateTime Expiration { get; set; }
	}
}
