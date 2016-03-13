using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public interface ISubscription
    {
        string DocumentID { get; set; }
        string Publication { get; set; }
        string SubscriptionType { get; set; }
        DateTime ExpirationDate { get; set; }
        string ProductCode { get; set; }
        string ProductGuid { get; set; }
        string ProductType { get; set; }
    }
}
