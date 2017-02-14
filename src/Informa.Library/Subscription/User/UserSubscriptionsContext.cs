using Informa.Library.SalesforceConfiguration;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Subscription.User
{
    [AutowireService(LifetimeScope.PerScope)]
    public class UserSubscriptionsContext : IUserSubscriptionsContext
    {
        private const string subscriptionsSessionKey = nameof(UserSubscriptionsContext);

        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IAuthenticatedUserSession UserSession;
        protected readonly IFindUserSubscriptions FindSubscriptions;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISalesforceConfigurationContext SalesforceContext;
        protected readonly IAuthenticatedUserEntitlementsContext AuthenticatedUserEntitlementsContext;

        public UserSubscriptionsContext(
            IAuthenticatedUserContext userContext,
            IAuthenticatedUserSession userSession,
            IFindUserSubscriptions findSubscriptions,
            ISiteRootContext siterootContext,
            IGlobalSitecoreService globalService,
            ISalesforceConfigurationContext salesforceContext,
            IAuthenticatedUserEntitlementsContext authenticatedUserEntitlementsContext)
        {
            UserContext = userContext;
            UserSession = userSession;
            FindSubscriptions = findSubscriptions;
            SiterootContext = siterootContext;
            GlobalService = globalService;
            SalesforceContext = salesforceContext;
            AuthenticatedUserEntitlementsContext = authenticatedUserEntitlementsContext;
        }

        public IEnumerable<ISubscription> Subscriptions
        {
            get
            {
                if (!UserContext.IsAuthenticated)
                {
                    return Enumerable.Empty<ISubscription>();
                }
                //Commenting it for the time being for local verification.
                var subscriptionSession = UserSession.Get<IEnumerable<ISubscription>>
                    (subscriptionsSessionKey);

                if (subscriptionSession.HasValue)
                {
                    return subscriptionSession.Value;
                }

                var subscriptions = Subscriptions =
                   SalesforceContext.IsNewSalesforceEnabled ?
                   AuthenticatedUserEntitlementsContext?.Entitlements?.Select(entitlement => Create(entitlement)
                   as ISubscription) : FindSubscriptions.Find(UserContext.User?.Username);

                return subscriptions;
            }
            set
            {
                UserSession.Set(subscriptionsSessionKey, value);
            }
        }

        private Subscription Create(IEntitlement model)
        {
            return new Subscription
            {
                SubscriptionType = model.Type,
                DocumentID = model.DocumentId,
                ProductCode = model.ProductCode,
                ProductGuid = model.ProductId,
                ProductType = model.ProductType,
                ExpirationDate = Convert.ToDateTime(model.SalesEndDate),
                Publication = model.Name
            };
        }
    }
}
