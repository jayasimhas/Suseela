using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Registration
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MockUsernameValidator : IUsernameValidator
	{
		public bool Validate(string username)
		{
			return !string.IsNullOrWhiteSpace(username) && username.Contains("@");
		}
	}
}
