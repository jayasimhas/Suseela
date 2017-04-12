using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
    public interface IFindSiteNewsletterUserOptInsContext
    {
        IEnumerable<INewsletterUserOptIn> Find(ISiteNewsletterTypes newsletterTypes);
        IEnumerable<INewsletterUserOptIn> FindDailyOptins(ISiteNewsletterTypes newsletterTypes);
        IEnumerable<INewsletterUserOptIn> FindWeeklyOptins(ISiteNewsletterTypes newsletterTypes);

    }
}