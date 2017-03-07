using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Newsletter
{
    public interface IOffersOptIn
    {
        bool OptIn { get; set; }
        string SalesforceId { get; set; }
    }
}
