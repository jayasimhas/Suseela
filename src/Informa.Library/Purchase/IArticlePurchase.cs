using System;

namespace Informa.Library.Purchase
{
	public interface IArticlePurchase
	{
		string Publication { get; }
		string DocumentId { get; }
		DateTime Expiration { get; }
	}
}
