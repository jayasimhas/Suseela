using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Orders;

namespace Informa.Library.Salesforce.User.Orders {
    public class SalesforceUserOrder : IUserOrder {
        
        protected readonly ISalesforceServiceContext Service;

        public SalesforceUserOrder(
            ISalesforceServiceContext service) {
            Service = service;
        }

        public ICreateUserOrderResult CreateUserOrder(IAuthenticatedUser user, IEnumerable<ISubscription> subscriptions) {
        
            if (user == null) 
                return CreateResult(false, Enumerable.Empty<string>());
            
            var createProfileRequest = new EBI_Order {
                account = new EBI_AccountData() { 
                    accountId = GetAccountID(user),
                    accountType = "Individual",
                },
                orderDate = DateTime.Now,
                orderDateSpecified = true,
                username = user.Username
            };

            var response = Service.Execute(s => s.createOrder(createProfileRequest));

            return CreateResult(response.IsSuccess(), response.errors.Select(a => a.message));
        }

        private string GetAccountID(IAuthenticatedUser user)
        {
            return (user.AccountId != null && user.AccountId.Any()) ? user.AccountId.First() : null;
        }

        private string GetSubscriptionType(IEnumerable<ISubscription> subscriptions)
        {
            return subscriptions?.OrderByDescending(o => o.ExpirationDate).FirstOrDefault().SubscriptionType ?? string.Empty;
        }

        private SalesforceCreateUserOrderResult CreateResult(bool success, IEnumerable<string> errors) {
            return new SalesforceCreateUserOrderResult {
                Errors = errors,
                Success = success
            };
        }
    }
}