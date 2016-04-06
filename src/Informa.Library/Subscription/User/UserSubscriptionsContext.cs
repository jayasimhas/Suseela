using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Informa.Library.Subscription.User
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserSubscriptionsContext : IUserSubscriptionsContext
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IManageSubscriptions ManageSubscriptions;
		protected readonly ISubscriptionProductKeyContext SubscriptionProductKeyContext;

		public UserSubscriptionsContext(
			IAuthenticatedUserContext userContext,
			IManageSubscriptions manageSubscriptions,
			ISubscriptionProductKeyContext subscriptionProductKeyContext)
		{
			UserContext = userContext;
			ManageSubscriptions = manageSubscriptions;
			SubscriptionProductKeyContext = subscriptionProductKeyContext;
		}

		public IEnumerable<ISubscription> Subscriptions
		{
			get
			{
				var result = ManageSubscriptions.QueryItems(UserContext.User);
				var subscriptions = (result.Success)
					? result.Subscriptions.Where(s => s.ProductType.Equals(SubscriptionProductKeyContext.ProductKey))
					: Enumerable.Empty<ISubscription>();
				return subscriptions;
			}
		}
	}
}
