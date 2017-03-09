using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

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

		public bool Set(IEnumerable<string> publications, bool isUpdate)
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
                if (!isUpdate)
                {
                    foreach (var siteTypes in SitesNewsletterTypes.SiteTypes)
                    {
                        if (publications.Contains(siteTypes.Publication.Code))
                        {
                            OptIns.Add(new NewsletterUserOptIn
                            {
                                NewsletterType = siteTypes.Publication.Code,
                                OptIn = true,
                            });
                        }
                        else
                        {
                            OptIns.Add(new NewsletterUserOptIn
                            {
                                NewsletterType = siteTypes.Publication.Code,
                                OptIn = false,
                            });
                        }
                    }
                    OptIns.ForEach(noi => noi.OptIn = newsletterTypes.Contains(noi.NewsletterType));
                    return SetNewsletterOptIns.Add(OptIns);
                    
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
