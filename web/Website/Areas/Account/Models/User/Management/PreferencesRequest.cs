using System.Collections.Generic;
using Informa.Library.User.Newsletter;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class PreferencesRequest
    {
        public string[] Publications { get; set; }
        public IEnumerable<Publications> NewsletterPublications { get; set; }
        public bool DoNotSendOffersOptIn { get; set; }
    }

}