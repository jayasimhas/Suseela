using Informa.Library.Purchase.User;
using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Publication;
using Informa.Library.Purchase;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.References;

namespace Informa.Library.Salesforce.Purchase.User
{
	public class SalesforceFindUserArticlePurchases : IFindUserArticlePurchases
	{
		protected readonly ISalesforceServiceContext Service;
		protected readonly IGlobalSitecoreService GlobalService;

		public SalesforceFindUserArticlePurchases(
			ISalesforceServiceContext service,
			IGlobalSitecoreService globalService)
		{
			Service = service;
			GlobalService = globalService;
		}

		public IEnumerable<IArticlePurchase> Find(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				return Enumerable.Empty<IArticlePurchase>();
			}

			var response = Service.Execute(s => s.INquerySubscriptionsAndPurchases(username));

			if (!response.IsSuccess())
			{
				return Enumerable.Empty<IArticlePurchase>();
			}

			if (response.subscriptionsAndPurchases == null || !response.subscriptionsAndPurchases.Any())
			{
				return Enumerable.Empty<IArticlePurchase>();
			}

			return response.subscriptionsAndPurchases
				.Where( sap => !string.IsNullOrEmpty(sap.documentId) && string.Equals(sap.productType, "article", StringComparison.InvariantCultureIgnoreCase))
				.Select(BuildSalesforceArticlePurchase)
				.ToList();
		}

		private IArticlePurchase BuildSalesforceArticlePurchase(EBI_SubscriptionAndPurchase purchase)
		{
			var sap = new SalesforceArticlePurchase();
			sap.DocumentId = purchase.documentId;
			sap.Expiration = (purchase.expirationDateSpecified) ? purchase.expirationDate.Value : DateTime.Now;

			// To get the Publication name we have to lookup the publication root based on the article number prefix. 
			// The fastest way to do this is to use the prefix to siteroot dictionary in the constants class
			var articlePrefix = string.IsNullOrEmpty(purchase.documentId) || purchase.documentId.Length < 8 ? "" : purchase.documentId.Substring(0, 2);
			Guid siteRoot = Guid.Empty;
			foreach (var entry in Constants.PublicationPrefixDictionary)
			{
				if (entry.Value.ToLowerInvariant() == articlePrefix.ToLowerInvariant())
				{
					siteRoot = entry.Key;
				}
			}
			sap.Publication = GlobalService.GetPublicationName(siteRoot);
			return sap;
		}
	}
}