using System.Collections.Generic;

namespace Informa.Library.Purchase.User
{
	public interface IUserArticlePurchasesContext
	{
		IEnumerable<IArticlePurchase> ArticlesPurchases { get; set; }
	}
}