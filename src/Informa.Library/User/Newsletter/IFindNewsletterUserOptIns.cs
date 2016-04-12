using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
    public interface IFindNewsletterUserOptIns
    {
        IEnumerable<INewsletterUserOptIn> Find(string username);
    }
}
