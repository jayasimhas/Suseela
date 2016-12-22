using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
   public class ChannelSubscription:IChannelSubscription
    {
        public string _ChannelId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<TopicSubscription> SubscribedTopics { get; set; }
    }
}
