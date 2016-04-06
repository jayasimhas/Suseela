﻿using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class UserEntitlementsContext : IUserEntitlementsContext
	{
		protected readonly IEnumerable<IEntitlementsContext> EntitlementsContexts;

		public UserEntitlementsContext(
			IEnumerable<IEntitlementsContext> entitlementsContexts)
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
