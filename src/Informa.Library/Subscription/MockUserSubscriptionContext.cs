using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Subscription
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MockUserSubscriptionContext : IUserSubscriptionContext
	{
		public bool IsSubscribed => false;
	}
}
