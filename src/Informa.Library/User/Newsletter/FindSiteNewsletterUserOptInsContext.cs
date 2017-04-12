using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class FindSiteNewsletterUserOptInsContext : IFindSiteNewsletterUserOptInsContext
    {
        protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;

        public FindSiteNewsletterUserOptInsContext(
            INewsletterUserOptInsContext newsletterUserOptInsContext)
        {
            NewsletterUserOptInsContext = newsletterUserOptInsContext;
        }

        public IEnumerable<INewsletterUserOptIn> Find(ISiteNewsletterTypes newsletterTypes)
        {
            return NewsletterUserOptInsContext.OptIns.Where(noi => IsSiteMatch(noi.NewsletterType, newsletterTypes));
        }

        public IEnumerable<INewsletterUserOptIn> FindDailyOptins(ISiteNewsletterTypes newsletterTypes)
        {
            return NewsletterUserOptInsContext.OptIns.Where(noi => IsDailySiteMatch(noi.NewsletterType, newsletterTypes));
        }

        public IEnumerable<INewsletterUserOptIn> FindWeeklyOptins(ISiteNewsletterTypes newsletterTypes)
        {
            return NewsletterUserOptInsContext.OptIns.Where(noi => IsWeeklSiteMatch(noi.NewsletterType, newsletterTypes));
        }

        public bool IsSiteMatch(string newsletterType, ISiteNewsletterTypes newsletterTypes)
        {
            return
                IsMatch(newsletterType, newsletterTypes.Breaking) ||
                IsMatch(newsletterType, newsletterTypes.Daily) ||
                IsMatch(newsletterType, newsletterTypes.Weekly);
        }

        public bool IsDailySiteMatch(string newsletterType, ISiteNewsletterTypes newsletterTypes)
        {
            return
                IsMatch(newsletterType, newsletterTypes.Breaking) ||
                IsMatch(newsletterType, newsletterTypes.Daily);

        }

        public bool IsWeeklSiteMatch(string newsletterType, ISiteNewsletterTypes newsletterTypes)
        {
            return IsMatch(newsletterType, newsletterTypes.Weekly);
        }

        public bool IsMatch(string newsletterType1, string newsletterType2)
        {
            return string.Equals(newsletterType1, newsletterType2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
