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

		public string GetSubscribed_Products()
		{
			if (Subscriptions != null && Subscriptions.Any())
			{
				StringBuilder strSubscription = new StringBuilder();
				var lastSubscription = Subscriptions.LastOrDefault();
				strSubscription.Append("[");
				foreach (var subscription in Subscriptions)
				{
					strSubscription.Append("'");
					strSubscription.Append(subscription.ProductCode);
					strSubscription.Append("'");
					if (Subscriptions.Count() > 1 && !lastSubscription.Equals(subscription))
					{
						strSubscription.Append(",");
					}
				}
				return strSubscription.ToString();
			}
			return string.Empty;
		}


	}
}
