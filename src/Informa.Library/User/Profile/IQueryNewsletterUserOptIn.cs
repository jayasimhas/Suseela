using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Authentication;

namespace Informa.Library.User.Profile
{
    public interface IQueryNewsletterUserOptIn
    {
        IQueryNewsletterUserOptInResult Query(IAuthenticatedUser user);
    }
}
