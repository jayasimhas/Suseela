using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class DefaultEntitlementsFactory : IDefaultEntitlementsFactory
	{
		public IEnumerable<IEntitlement> Create()
		{
			return Enumerable.Empty<IEntitlement>();
		}
	}
}
