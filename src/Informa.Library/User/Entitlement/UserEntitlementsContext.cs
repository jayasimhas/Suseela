using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class UserEntitlementsContext : IUserEntitlementsContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;

		public UserEntitlementsContext(
			ISitecoreUserContext sitecoreUserContext)
		{
			SitecoreUserContext = sitecoreUserContext;
		}

		public IEnumerable<IEntitlement> Entitlements => SitecoreUserContext.Entitlements;
	}
}
