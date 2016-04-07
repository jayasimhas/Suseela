using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class DefaultEntitlementsFactory : IDefaultEntitlementsFactory
	{
		public IEnumerable<IEntitlement> Create()
		{
			return new List<IEntitlement> { new Entitlement { ProductCode = "NONE" } };
		}
	}
}
