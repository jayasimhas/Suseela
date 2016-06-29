using System.Collections.Generic;

namespace Informa.Library.Subscription.User
{
    public interface IFindUserSubscriptions
	{
		IEnumerable<ISubscription> Find(string username);
    }
}
