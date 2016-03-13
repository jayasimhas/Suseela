using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class AccountInfoWriteResult : IAccountInfoWriteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
