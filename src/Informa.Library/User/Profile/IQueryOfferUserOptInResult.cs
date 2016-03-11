using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public interface IQueryOfferUserOptInResult
    {
        bool Success { get; set; }
        bool DoNotSendOffers { get; set; }
    }
}
