using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Orders {
    public interface ICreateUserOrderResult {
        bool Success { get; }
        IEnumerable<string> Errors { get; }
    }
}
