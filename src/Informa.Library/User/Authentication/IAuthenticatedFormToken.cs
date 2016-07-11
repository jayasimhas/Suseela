using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Authentication
{
    public interface IAuthenticatedFormToken
    {
        string GenerateMD5Token();
        string currentToken { get; set; }
    }
}
