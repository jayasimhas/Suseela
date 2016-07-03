using System.Collections.Generic;
using Informa.Library.Subscription;

namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IUserSubscriptionsViewModel
	{
		IEnumerable<ISubscription> Subscriptions { get; }
	}
}