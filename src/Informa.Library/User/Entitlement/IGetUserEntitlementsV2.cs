using Informa.Library.User.Authentication;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IGetUserEntitlementsV2
    {
        IList<IEntitlement> GetEntitlements(IAuthenticatedUser user);
    }
}