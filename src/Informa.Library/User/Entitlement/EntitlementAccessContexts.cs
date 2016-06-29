using System.Collections;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	public class EntitlementAccessContexts : IEntitlementAccessContexts
	{
		protected readonly IEnumerable<IEntitlementAccessContext> Contexts;

		public EntitlementAccessContexts(
			IEnumerable<IEntitlementAccessContext> contexts)
		{
			Contexts = contexts;
		}

		public IEnumerator<IEntitlementAccessContext> GetEnumerator()
		{
			return Contexts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
