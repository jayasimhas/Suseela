using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Corporate
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MockCorporateAccountNameContext : ICorporateAccountNameContext
	{
		public string Name => string.Empty;
	}
}
