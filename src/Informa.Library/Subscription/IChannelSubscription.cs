using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
    public interface IChannelSubscription
    {
        string _ChannelId { get; set; }
        string ChannelId { get; set; }
        string ChannelName { get; set; }
        DateTime ExpirationDate { get; set; }
        List<TopicSubscription> SubscribedTopics { get; set; }
        }
}
