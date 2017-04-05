using Informa.Library.SalesforceConfiguration;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Purchase.User
{
	[AutowireService]
	public class UserArticlePurchaseItemsContext : IUserArticlePurchaseItemsContext
	{
		protected readonly IUserArticlePurchasesContext ArticlePurchasesContext;
		protected readonly IArticlePurchaseItemsFactory ArticlePurchaseItemFactory;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;


        public UserArticlePurchaseItemsContext(
			IUserArticlePurchasesContext articlePurchasesContext,
			IArticlePurchaseItemsFactory articlePurchaseItemFactory,
            ISalesforceConfigurationContext salesforceConfigurationContext)
		{
			ArticlePurchasesContext = articlePurchasesContext;
			ArticlePurchaseItemFactory = articlePurchaseItemFactory;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

		public IEnumerable<IArticlePurchaseItem> ArticlePurchaseItems =>
            SalesforceConfigurationContext.IsNewSalesforceEnabled ?
            Enumerable.Empty<IArticlePurchaseItem>():
        ArticlePurchaseItemFactory.Create(ArticlePurchasesContext.ArticlesPurchases);
	}
}
