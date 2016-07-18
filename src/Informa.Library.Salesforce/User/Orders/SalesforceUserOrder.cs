using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;
using Informa.Library.User.Orders;
using Informa.Library.Site;

namespace Informa.Library.Salesforce.User.Orders {
    public class SalesforceUserOrder : IUserOrder {

        protected readonly ISalesforceServiceContext Service;
        protected readonly ISiteRootContext SiteRoot;

        public SalesforceUserOrder(
            ISalesforceServiceContext service,
            ISiteRootContext siteRoot) {
            Service = service;
            SiteRoot = siteRoot;
        }

        public ICreateUserOrderResult CreateUserOrder(IAuthenticatedUser user, IEnumerable<ISubscription> subscriptions) {

            if (user == null)
                return CreateResult(false, Enumerable.Empty<string>());
            
            var orderItem = new EBI_OrderItem() {
                entitlementAccessType = "Online Only",
                entitlementArchiveCode = "90",
                entitlementLicenseType = "Named User",
                entitlementProductCode = SiteRoot.Item.Publication_Code,
                entitlementProductType = "Publication",
                entitlementType = "Free Trial",
                productGUID = SiteRoot.Item._Id.ToString(),
                productRatePlanChargeId = SiteRoot.Item.Free_Trial_Rate_Plan_Charge_Id,
                productRatePlanId = SiteRoot.Item.Free_Trial_Rate_Plan_Id,
                quantity = "1"
            };

            var createProfileRequest = new EBI_Order {
                subscriptionId = SiteRoot.Item.Publication_Code,
                account = new EBI_AccountData() {
                    accountId = GetAccountID(user),
                    accountType = "Individual",
                },
                orderDate = DateTime.Now,
                orderDateSpecified = true,
                username = user.Username,
                orderItems = new EBI_OrderItem[] { orderItem }
            };

            var response = Service.Execute(s => s.createOrder(createProfileRequest));

            return CreateResult(response.IsSuccess(), response.errors.Select(a => a.message));
        }

        private string GetAccountID(IAuthenticatedUser user) {
            return (user.AccountId != null && user.AccountId.Any()) ? user.AccountId.First() : null;
        }

        private string GetSubscriptionType(IEnumerable<ISubscription> subscriptions) {
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