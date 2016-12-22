using System;
using System.Collections.Generic;

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
        List<ChannelSubscription> SubscribedChannels { get; set; }
        bool IsTopicSubscription { get; set; }
        //List<TopicSubscription> SubscribedTopics { get; set; }

    }
}
