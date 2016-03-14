using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public interface ISubscriptionsReadResult
    {
        bool Success { get; set; }
        IEnumerable<ISubscription> Subscriptions { get; set; } 
    }
}
