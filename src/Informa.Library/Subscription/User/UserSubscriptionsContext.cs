using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;
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
		protected readonly IManageSubscriptions ManageSubscriptions;
		protected readonly ISubscriptionProductKeyContext SubscriptionProductKeyContext;

		public UserSubscriptionsContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IManageSubscriptions manageSubscriptions,
			ISubscriptionProductKeyContext subscriptionProductKeyContext)
		{
			UserContext = userContext;
			UserSession = userSession;
			ManageSubscriptions = manageSubscriptions;
			SubscriptionProductKeyContext = subscriptionProductKeyContext;
		}

		public IEnumerable<ISubscription> Subscriptions
		{
			get
			{
				var subscriptionSession = UserSession.Get<IEnumerable<ISubscription>>(subscriptionsSessionKey);

				if (subscriptionSession.HasValue)
				{
					return subscriptionSession.Value;
				}

				var result = ManageSubscriptions.QueryItems(UserContext.User);
				var subscriptions = (result.Success)
					? result.Subscriptions.Where(s => s.ProductType.Equals(SubscriptionProductKeyContext.ProductKey))
					: Enumerable.Empty<ISubscription>();

				Subscriptions = subscriptions;

				return subscriptions;
			}
			set
			{
				UserSession.Set(subscriptionsSessionKey, value);
			}
		}
	}
}
