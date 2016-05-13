using System;

namespace Informa.Library.Purchase
{
	public class ArticlePurchaseItem : IArticlePurchaseItem
	{
		public string Title { get; set; }
		public DateTime Expiration { get; set; }
		public string Url { get; set; }
		public string Publication { get; set; }
		public bool IsExternalUrl { get; set; }
	}
}