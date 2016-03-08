using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class NewsletterOptIn : INewsletterOptIn
    {
        public string Name { get; set; }
        public bool ReceivesEmailAlert { get; set; }
        public bool ReceivesNewsletterAlert { get; set; }
        public string Frequency { get; set; }
    }
}
