using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
    public class TopicSubscription : ITopicSubscription
    {
        public string TopicId { get; set; }
        public string TopicName { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
