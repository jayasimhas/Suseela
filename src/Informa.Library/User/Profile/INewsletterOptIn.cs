using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public interface INewsletterOptIn
    {
        string Name { get; set; }
        bool ReceivesEmailAlert { get; set; }
        bool ReceivesNewsletterAlert { get; set; }
        string Frequency { get; set; }
    }
}
