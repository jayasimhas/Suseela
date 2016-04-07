using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
    public interface IFindNewsletterUserOptIn
    {
        IEnumerable<INewsletterUserOptIn> Find(string username);
    }
}
