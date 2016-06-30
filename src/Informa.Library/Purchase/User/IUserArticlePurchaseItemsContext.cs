using System.Collections.Generic;

namespace Informa.Library.Purchase.User
{
	public interface IUserArticlePurchaseItemsContext
	{
		IEnumerable<IArticlePurchaseItem> ArticlePurchaseItems { get; }
	}
}