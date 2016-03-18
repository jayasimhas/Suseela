using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
    public interface IIndividualSubscriptionRenewalMessageContext
    {
        string ID { get; }
        string Message_IndividualSubscriptiong { get; }
        string Message_FreeTrial { get; }
        string RenewalLinkURL { get; }
        string RenewalLinkText { get; }
    }
}
