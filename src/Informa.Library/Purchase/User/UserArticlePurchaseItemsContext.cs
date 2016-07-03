using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.Purchase.User
{
	[AutowireService]
	public class UserArticlePurchaseItemsContext : IUserArticlePurchaseItemsContext
	{
		protected readonly IUserArticlePurchasesContext ArticlePurchasesContext;
		protected readonly IArticlePurchaseItemsFactory ArticlePurchaseItemFactory;

		public UserArticlePurchaseItemsContext(
			IUserArticlePurchasesContext articlePurchasesContext,
			IArticlePurchaseItemsFactory articlePurchaseItemFactory)
		{
			ArticlePurchasesContext = articlePurchasesContext;
			ArticlePurchaseItemFactory = articlePurchaseItemFactory;
		}

		public IEnumerable<IArticlePurchaseItem> ArticlePurchaseItems => ArticlePurchaseItemFactory.Create(ArticlePurchasesContext.ArticlesPurchases);
	}
}
