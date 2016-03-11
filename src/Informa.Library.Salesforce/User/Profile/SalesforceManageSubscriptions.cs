using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceManageSubscriptions : IManageSubscriptions
    {
        protected readonly ISalesforceServiceContext Service;
        protected readonly ITextTranslator TextTranslator;
        
        public SalesforceManageSubscriptions(
            ISalesforceServiceContext service,
            ITextTranslator textTranslator)
        {
            Service = service;
            TextTranslator = textTranslator;
        }

        public ISubscriptionsReadResult QueryItems(IAuthenticatedUser user)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return ReadErrorResult;
            }

            var response = Service.Execute(s => s.querySubscriptionsAndPurchases(user.Email));

            if (!response.IsSuccess())
            {
                return ReadErrorResult;
            }

            return new SubscriptionsReadResult
            {
                Success = true,
                Subscriptions = (response.subscriptionsAndPurchases != null && response.subscriptionsAndPurchases.Any())
                    ? response.subscriptionsAndPurchases.Select(a => new Library.User.Profile.Subscription()
                    {
                        DocumentID = a.documentId,
                        ProductCode = a.productCode,
                        ProductGuid = a.productGUID,
                        SubscriptionType = a.subscriptionType,
                        Publication = a.name,
                        ExpirationDate = (a.expirationDateSpecified) ? a.expirationDate.Value : DateTime.Now
                    })
                    : Enumerable.Empty<ISubscription>()
            };
        }
        
        public ISubscriptionsReadResult ReadErrorResult => new SubscriptionsReadResult
        {
            Success = false,
            Subscriptions = Enumerable.Empty<ISubscription>()
        };
    }
}
