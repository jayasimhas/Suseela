using Informa.Library.User.Entitlement;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Linq;

namespace Informa.Library.Purchase.User.Entitlement
{
	[AutowireService]
	public class PurchaseEntitlementAccessContext : IEntitlementAccessContext
	{
		protected readonly IUserArticlePurchasesContext ArticlePurchasesContext;
		protected readonly IEntitlementAccessFactory EntitlementAccessFactory;

		public PurchaseEntitlementAccessContext(
			IUserArticlePurchasesContext articlePurchasesContext,
			IEntitlementAccessFactory entitlementAccessFactory)
		{
			ArticlePurchasesContext = articlePurchasesContext;
			EntitlementAccessFactory = entitlementAccessFactory;
		}

		public IEntitlementAccess Find(IEntitledProduct entitledProduct)
		{
			var matchingPurchase = ArticlePurchasesContext.ArticlesPurchases?.FirstOrDefault(ap => string.Equals(ap.DocumentId, entitledProduct.DocumentId, StringComparison.InvariantCultureIgnoreCase));

			if (matchingPurchase == null || entitledProduct.PublishedOn >= matchingPurchase.Expiration)
			{
				return Create(EntitledAccessLevel.None);
			}

			return Create(EntitledAccessLevel.Individual);
		}

		public IEntitlementAccess Create(EntitledAccessLevel accessLevel)
		{
			return EntitlementAccessFactory.Create(null, accessLevel);
		}
	}
}
