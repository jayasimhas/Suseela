using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Newsletter;

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
            var newsletterTypes = new List<string>();
            var OptIns = new List<INewsletterUserOptIn>();

            foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => publications.Any(p => string.Equals(p, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
            {
                AddType(siteTypes.Breaking, ref newsletterTypes);
                AddType(siteTypes.Daily, ref newsletterTypes);
                AddType(siteTypes.Weekly, ref newsletterTypes);
            }
            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                foreach (var newsletteritem in newsletterTypes)
                {
                    OptIns.Add(new NewsletterUserOptIn
                    {
                        NewsletterType = newsletteritem,
                        OptIn = true
                    });
                }
                return SetNewsletterOptIns.Add(OptIns);
            }
            else
            {
                return SetNewsletterOptIns.Set(newsletterTypes);
            }

        }


        public bool SetUpdate(IEnumerable<Publications> Publication)
        {
            var newsletterTypes = new List<string>();

            if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
            {
                foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => Publication.Any(p => p.DailyPublications &&
                string.Equals(p.publication, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    AddType(siteTypes.Breaking, ref newsletterTypes);
                    AddType(siteTypes.Daily, ref newsletterTypes);
                }

                foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => Publication.Any(p => p.WeeklyPublications &&
                string.Equals(p.publication, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    AddType(siteTypes.Weekly, ref newsletterTypes);
                }
            }
            else
            {
                foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => Publication.Any(p =>p.OptIns && 
                string.Equals(p.publication, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
                {
                    AddType(siteTypes.Breaking, ref newsletterTypes);
                    AddType(siteTypes.Daily, ref newsletterTypes);
                    AddType(siteTypes.Weekly, ref newsletterTypes);
                }
            }

            return SetNewsletterOptIns.Set(newsletterTypes);

        }

        public void AddType(string newsletterType, ref List<string> newsletterTypes)
        {
            if (!string.IsNullOrEmpty(newsletterType))
            {
                newsletterTypes.Add(newsletterType);
            }
        }
    }
}
