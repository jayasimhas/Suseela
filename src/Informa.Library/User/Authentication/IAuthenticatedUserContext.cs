using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Library.User.Authentication
{
	public interface IAuthenticatedUserContext
	{
		IAuthenticatedUser User { get; }
		bool IsAuthenticated { get; }
        IList<IEntitlement> Entitlements { get; } 
	}
}
