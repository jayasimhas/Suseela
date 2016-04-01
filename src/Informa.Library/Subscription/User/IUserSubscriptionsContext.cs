using System.Collections.Generic;
using Informa.Library.User.Profile;

namespace Informa.Library.Subscription.User
{
	public interface IUserSubscriptionsContext
	{
		IEnumerable<ISubscription> Subscriptions { get; }
		string GetSubscribed_Products();
	}
}