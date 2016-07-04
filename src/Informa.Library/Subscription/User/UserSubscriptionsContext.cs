using Informa.Library.User.Authentication;
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
		protected readonly IFindUserSubscriptions FindSubscriptions;

		public UserSubscriptionsContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IFindUserSubscriptions findSubscriptions)
		{
			UserContext = userContext;
			UserSession = userSession;
			FindSubscriptions = findSubscriptions;
		}

		public IEnumerable<ISubscription> Subscriptions
		{
			get
			{
			    if (!UserContext.IsAuthenticated)
			    {
			        return Enumerable.Empty<ISubscription>();
			    }

				var subscriptionSession = UserSession.Get<IEnumerable<ISubscription>>(subscriptionsSessionKey);

				if (subscriptionSession.HasValue)
				{
					return subscriptionSession.Value;
				}

				var subscriptions = Subscriptions = FindSubscriptions.Find(UserContext.User?.Username);

				return subscriptions;
			}
			set
			{
				UserSession.Set(subscriptionsSessionKey, value);
			}
		}
	}
}
