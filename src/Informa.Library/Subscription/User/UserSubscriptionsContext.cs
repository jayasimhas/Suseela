using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.Subscription.User
{
	[AutowireService(LifetimeScope.PerScope)]
	public class UserSubscriptionsContext : IUserSubscriptionsContext
	{
		private const string subscriptionsSessionKey = nameof(UserSubscriptionsContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IFindUserSubscriptions ManageSubscriptions;

		public UserSubscriptionsContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IFindUserSubscriptions manageSubscriptions)
		{
			UserContext = userContext;
			UserSession = userSession;
			ManageSubscriptions = manageSubscriptions;
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

				var subscriptions = Subscriptions = ManageSubscriptions.Find(UserContext.User.Username);

				return subscriptions;
			}
			set
			{
				UserSession.Set(subscriptionsSessionKey, value);
			}
		}
	}
}
