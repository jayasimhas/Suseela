using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Authentication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MockUserAuthenticationContext : IUserAuthenticationContext
	{
		public bool IsAuthenticated => false; // TODO: Implement
	}
}
