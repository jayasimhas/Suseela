using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.Subscription.User
{
	[AutowireService(LifetimeScope.Default)]
	public class UserSubscribedContext : IUserSubscribedContext
	{
		protected readonly IUserSubscriptionsContext UserSubscriptionsContext;

		public UserSubscribedContext(
			IUserSubscriptionsContext userSubscriptionsContext)
		{
			UserSubscriptionsContext = userSubscriptionsContext;
		}

		public bool IsSubscribed => UserSubscriptionsContext.Subscriptions.Any();
	}
}
