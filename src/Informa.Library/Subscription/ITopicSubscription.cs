using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
    public interface ITopicSubscription
    {
        string TopicId { get; set; }
        string TopicName { get; set; }
        DateTime ExpirationDate { get; set; }

    }
}
