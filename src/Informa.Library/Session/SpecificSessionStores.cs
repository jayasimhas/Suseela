using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Session
{
	public class SpecificSessionStores : ISpecificSessionStores
	{
		protected readonly IEnumerable<ISpecificSessionStore> SessionStores;

		public SpecificSessionStores(
			IEnumerable<ISpecificSessionStore> sessionStores)
		{
			SessionStores = sessionStores;
		}

		public IEnumerator<ISpecificSessionStore> GetEnumerator()
		{
			return SessionStores.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Clear()
		{
			SessionStores.ToList().ForEach(ss => ss.Clear());
		}
	}
}
