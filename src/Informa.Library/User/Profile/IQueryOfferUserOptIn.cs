using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;

namespace Informa.Library.Salesforce.User.Profile
{
    public interface IQueryOfferUserOptIn
    {
        IQueryOfferUserOptInResult Query(IAuthenticatedUser user);
    }
}
