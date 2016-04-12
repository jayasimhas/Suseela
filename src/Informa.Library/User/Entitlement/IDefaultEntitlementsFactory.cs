using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	public interface IDefaultEntitlementsFactory
	{
		IEnumerable<IEntitlement> Create();
	}
}