using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Utilities.Extensions;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class UserEntitlementsContext : IUserEntitlementsContext
	{
		protected readonly IEntitlementsContexts EntitlementsContexts;

		public UserEntitlementsContext(
			IEntitlementsContexts entitlementsContexts)
		{
			EntitlementsContexts = entitlementsContexts;
		}

		public IEnumerable<IEntitlement> Entitlements => EntitlementsContexts.SelectMany(ec => ec.Entitlements).DistinctBy(e => e.ProductCode);

		public void RefreshEntitlements()
		{
			EntitlementsContexts.ToList().ForEach(ec => ec.RefreshEntitlements());
		}
	}
}
