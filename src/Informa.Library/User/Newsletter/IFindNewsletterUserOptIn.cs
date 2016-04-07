using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
    public interface IFindNewsletterUserOptIn
    {
        IEnumerable<INewsletterUserOptIn> Find(string username);
    }
}
