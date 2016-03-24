using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
	public interface IUserEntitlementsContext
	{
		IEnumerable<IEntitlement> Entitlements { get; }
	}
}