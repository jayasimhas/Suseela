using System.Collections.Generic;

namespace Informa.Library.Purchase.User
{
	public interface IFindUserArticlePurchases
	{
		IEnumerable<IArticlePurchase> Find(string username);
	}
}
