using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceQueryNewsletterUserOptInResult : IQueryNewsletterUserOptInResult
    {
        public bool Success { get; set; }
        public IEnumerable<INewsletterOptIn> NewsletterOptIns { get; set; } 
    }
}
