using System.Collections.Generic;

namespace Informa.Web.Areas.Account.Models.User.Registration
{
    public class SetOptInsRequest
    {
        public bool Offers { get; set; }
        public IEnumerable<PublicationNewslettersOptIns> Newsletters { get; set; }
    }

    public class PublicationNewslettersOptIns
    {
        public string PublicationCode { get; set; }
        public bool NewsletterChecked { get; set; }
    }
}