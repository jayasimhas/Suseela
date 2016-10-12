using System;
using System.Collections;
using System.Collections.Generic;
using Informa.Library.User.UserPreference;

namespace Informa.Library.Subscription
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
        IList<Channel> Channels { get; set; }
        IList<Topic> Topics { get; set; }
    }
}
