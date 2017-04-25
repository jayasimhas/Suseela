using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Newsletter;
using System.Collections.Specialized;

namespace Informa.Library.User.Newsletter
{
    [AutowireService]
    public class SetPublicationsNewsletterUserOptIns : ISetPublicationsNewsletterUserOptIns
    {
        protected readonly ISetByTypeNewsletterUserOptInsContext SetNewsletterOptIns;
        protected readonly ISitesNewsletterTypes SitesNewsletterTypes;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public SetPublicationsNewsletterUserOptIns(
            ISetByTypeNewsletterUserOptInsContext setNewsletterOptIns,
            ISitesNewsletterTypes sitesNewsletterTypes,
            ISalesforceConfigurationContext salesforceConfigurationContext
            )
        {
            SetNewsletterOptIns = setNewsletterOptIns;
            SitesNewsletterTypes = sitesNewsletterTypes;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public bool Set(IEnumerable<string> publications)
        {
            var newsletterTypes = new List<KeyValuePair<string, string>>();
            var OptIns = new List<INewsletterUserOptIn>();

            foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => publications.Any(p => string.Equals(p, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
            {
                AddType(siteTypes.Breaking, siteTypes.Publication.Code, ref newsletterTypes, ref OptIns);
                AddType(siteTypes.Daily, siteTypes.Publication.Code, ref newsletterTypes, ref OptIns);
                AddType(siteTypes.Weekly, siteTypes.Publication.Code, ref newsletterTypes, ref OptIns);
            }
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                return SetNewsletterOptIns.Add(OptIns);
            }
            else
            {
                return SetNewsletterOptIns.Set(newsletterTypes);
            }

        }


        public bool SetUpdate(IEnumerable<Publications> Publication)
        {
            var newsletterTypes = new List<KeyValuePair<string, string>>();

            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => Publication.Any(p => p.DailyPublications &&
                string.Equals(p.publication, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    AddType(siteTypes.Breaking, siteTypes.Publication.Code, ref newsletterTypes);
                    AddType(siteTypes.Daily, siteTypes.Publication.Code, ref newsletterTypes);
                }

                foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => Publication.Any(p => p.WeeklyPublications &&
                string.Equals(p.publication, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    AddType(siteTypes.Weekly, siteTypes.Publication.Code, ref newsletterTypes);
                }
            }
            else
            {
                foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => Publication.Any(p => p.OptIns &&
                string.Equals(p.publication, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    AddType(siteTypes.Breaking, siteTypes.Publication.Code, ref newsletterTypes);
                    AddType(siteTypes.Daily, siteTypes.Publication.Code, ref newsletterTypes);
                    AddType(siteTypes.Weekly, siteTypes.Publication.Code, ref newsletterTypes);
                }
            }

            return SetNewsletterOptIns.Set(newsletterTypes);

        }

        public void AddType(string newsletterType, string publicationCode, ref List<KeyValuePair<string, string>> newsletterTypes)
        {
            if (!string.IsNullOrEmpty(newsletterType) && !string.IsNullOrEmpty(publicationCode))
            {
                newsletterTypes.Add(new KeyValuePair<string, string>(publicationCode, newsletterType));
            }
        }

        public void AddType(string newsletterType, string publicationCode,
            ref List<KeyValuePair<string, string>> newsletterTypes, ref List<INewsletterUserOptIn> OptIns)
        {
            if (!string.IsNullOrEmpty(newsletterType))
            {
                newsletterTypes.Add(new KeyValuePair<string, string>(publicationCode, newsletterType));
                if (SalesforceConfigurationContext.IsNewSalesforceEnabled && !string.IsNullOrEmpty(publicationCode))
                    OptIns.Add(new NewsletterUserOptIn
                    {
                        NewsletterType = newsletterType,
                        PublicationCode = publicationCode,
                        OptIn = true
                    });
            }
        }
    }
}
