using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class SubscriptionsReadResult : ISubscriptionsReadResult
    {
        public bool Success { get; set; }
        public IEnumerable<ISubscription> Subscriptions { get; set; }
    }
}
