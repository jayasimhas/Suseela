using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledContext
    {
        bool IsEntitled(IEntitlement entitlement);
    }
}