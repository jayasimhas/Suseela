using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;

namespace Informa.Library.Salesforce.Subscription.User
{
    public class SalesforceFindUserSubscriptions : IFindUserSubscriptions
    {
        protected readonly ISalesforceServiceContext Service;
        
        public SalesforceFindUserSubscriptions(
            ISalesforceServiceContext service)
        {
            Service = service;
        }

		public IEnumerable<ISubscription> Find(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				return null;
			}

			var response = Service.Execute(s => s.INquerySubscriptionsAndPurchases(username));

			if (!response.IsSuccess())
			{
				return null;
			}

			if (response.subscriptionsAndPurchases == null || !response.subscriptionsAndPurchases.Any())
			{
				return Enumerable.Empty<ISubscription>();
			}

			return response.subscriptionsAndPurchases
				.Where(sap => string.Equals(sap.productType, "publication", StringComparison.InvariantCultureIgnoreCase))
				.Select(sap => new SalesforceSubscription
				{
					DocumentID = sap.documentId,
					ProductCode = sap.productCode,
					ProductGuid = sap.productGUID,
					SubscriptionType = sap.subscriptionType,
					Publication = sap.name,
					ProductType = sap.productType,
					ExpirationDate = (sap.expirationDateSpecified) ? sap.expirationDate.Value : DateTime.Now
				})
				.ToList();
		}
    }
}
