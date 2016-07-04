using System.Collections.Generic;

namespace Informa.Library.Purchase
{
	public interface IArticlePurchaseItemsFactory
	{
		IEnumerable<IArticlePurchaseItem> Create(IEnumerable<IArticlePurchase> articlePurchases);
	}
}