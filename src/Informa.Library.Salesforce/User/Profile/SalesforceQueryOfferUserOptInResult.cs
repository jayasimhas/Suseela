using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceQueryOfferUserOptInResult : IQueryOfferUserOptInResult
    {
        public bool Success { get; set; }
        public bool DoNotSendOffers { get; set; }
    }
}
