using System.Collections;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	public class EntitlementsContexts : IEntitlementsContexts
	{
		protected readonly IEnumerable<IEntitlementsContext> Contexts;

		public EntitlementsContexts(
			IEnumerable<IEntitlementsContext> contexts)
		{
			Contexts = contexts;
		}

		public IEnumerator<IEntitlementsContext> GetEnumerator()
		{
			return Contexts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
