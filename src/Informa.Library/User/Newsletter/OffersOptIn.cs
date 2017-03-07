using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Newsletter
{
    public class OffersOptIn : IOffersOptIn
    {
        public bool OptIn { get; set; }
        public string SalesforceId { get; set; }
    }
}
