using System.Collections.Generic;
using Informa.Library.User.Profile;

namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IUserSubscriptionsViewModel
	{
		IEnumerable<ISubscription> Subscriptions { get; }
	}
}