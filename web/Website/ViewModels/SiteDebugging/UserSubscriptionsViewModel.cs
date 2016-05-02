using Informa.Library.Subscription.User;
using Informa.Library.Subscription;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.Default)]
	public class UserSubscriptionsViewModel : IUserSubscriptionsViewModel
	{
		protected readonly IUserSubscriptionsContext UserSubscriptionsContext;

		public UserSubscriptionsViewModel(
			IUserSubscriptionsContext userSubscriptionsContext)
		{
			UserSubscriptionsContext = userSubscriptionsContext;	
		}

		public IEnumerable<ISubscription> Subscriptions => UserSubscriptionsContext.Subscriptions;
	}
}