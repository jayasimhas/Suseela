using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class Subscription : ISubscription
    {
        public string DocumentID { get; set; }
        public string Publication { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductGuid { get; set; }
    }
}
