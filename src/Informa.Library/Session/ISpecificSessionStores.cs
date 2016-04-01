using System.Collections.Generic;

namespace Informa.Library.Session
{
	public interface ISpecificSessionStores : IEnumerable<ISpecificSessionStore>
	{
		void Clear();
	}
}
