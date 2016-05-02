using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
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

			var response = Service.Execute(s => s.querySubscriptionsAndPurchases(username));

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
				.Select(a => new SalesforceSubscription
				{
					DocumentID = a.documentId,
					ProductCode = a.productCode,
					ProductGuid = a.productGUID,
					SubscriptionType = a.subscriptionType,
					Publication = a.name,
					ProductType = a.productType,
					ExpirationDate = (a.expirationDateSpecified) ? a.expirationDate.Value : DateTime.Now
				})
				.ToList();
		}
    }
}
