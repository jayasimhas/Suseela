using System;

namespace Informa.Library.Purchase
{
	public interface IArticlePurchaseItem
	{
		DateTime Expiration { get; }
		string Publication { get; }
		string Title { get; }
		string Url { get; }
	}
}