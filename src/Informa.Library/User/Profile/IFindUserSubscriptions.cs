using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
    public interface IFindUserSubscriptions
	{
		IEnumerable<ISubscription> Find(string username);
    }
}
