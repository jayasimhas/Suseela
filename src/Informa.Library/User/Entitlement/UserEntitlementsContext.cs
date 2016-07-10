using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class UserEntitlementsContext : IUserEntitlementsContext
	{
		protected readonly IEntitlementsContexts EntitlementsContexts;

		public UserEntitlementsContext(
			IEntitlementsContexts entitlementsContexts)
		{
			EntitlementsContexts = entitlementsContexts;
		}

		public IEnumerable<IEntitlement> Entitlements => EntitlementsContexts.SelectMany(ec => ec.Entitlements);

		public void RefreshEntitlements()
		{
			EntitlementsContexts.ToList().ForEach(ec => ec.RefreshEntitlements());
		}
	}
}
