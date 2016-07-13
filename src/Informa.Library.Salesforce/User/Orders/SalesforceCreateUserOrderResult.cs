using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Orders;

namespace Informa.Library.Salesforce.User.Orders {
    public class SalesforceCreateUserOrderResult : ICreateUserOrderResult {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
