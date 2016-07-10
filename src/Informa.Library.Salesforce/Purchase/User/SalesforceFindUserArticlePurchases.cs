using Informa.Library.Purchase.User;
using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Purchase;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Services.Global;

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
				.Where(sap => string.Equals(sap.productType, "article", StringComparison.InvariantCultureIgnoreCase))
				.Select(sap => new SalesforceArticlePurchase
				{
					DocumentId = sap.documentId,
					Expiration = (sap.expirationDateSpecified) ? sap.expirationDate.Value : DateTime.Now,
					Publication = GlobalService.GetPublicationName(new Guid(sap.documentId))
                })
				.ToList();
		}	    
	}
}
