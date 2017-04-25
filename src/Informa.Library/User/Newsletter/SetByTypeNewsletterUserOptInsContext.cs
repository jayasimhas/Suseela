using Informa.Library.SalesforceConfiguration;
using Jabberwocky.Autofac.Attributes;
using Sitecore.Common;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
    [AutowireService(LifetimeScope.Default)]
    public class SetByTypeNewsletterUserOptInsContext : ISetByTypeNewsletterUserOptInsContext
    {
        protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;
        protected readonly IUpdateNewsletterUserOptInsContext UpdateNewsletterOptInsContext;
        protected readonly IAddNewsletterUserOptInsContext AddNewsletterUserOptInsContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public SetByTypeNewsletterUserOptInsContext(
            INewsletterUserOptInsContext newsletterUserOptInsContext,
            IUpdateNewsletterUserOptInsContext updateNewsletterOptInsContext,
            IAddNewsletterUserOptInsContext addNewsletterUserOptInsContext,
            ISalesforceConfigurationContext salesforceConfigurationContext
            )
        {
            NewsletterUserOptInsContext = newsletterUserOptInsContext;
            UpdateNewsletterOptInsContext = updateNewsletterOptInsContext;
            AddNewsletterUserOptInsContext = addNewsletterUserOptInsContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public bool Set(List<KeyValuePair<string, string>> newsletterTypes)
        {
            if (newsletterTypes != null && newsletterTypes.Any())
            {
                var emailnewsletterTypes = new List<string>();
                var OptIns = new List<INewsletterUserOptIn>();

                var newsletterUserOptins = NewsletterUserOptInsContext.OptIns.ToList();
                newsletterUserOptins.ForEach(noi => noi.OptIn = newsletterTypes.Select(x => x.Value).Contains(noi.NewsletterType));

                if (SalesforceConfigurationContext.IsNewSalesforceEnabled)
                {
                    //Newly selected Newsletter
                    emailnewsletterTypes = newsletterTypes.Select(x => x.Value).Except(newsletterUserOptins.Select(s => s.NewsletterType)).ToList();
                    if (emailnewsletterTypes != null && emailnewsletterTypes.Count() > 0)
                    {
                        foreach (var newsletteritem in emailnewsletterTypes)
                        {
                            OptIns.Add(new NewsletterUserOptIn
                            {
                                NewsletterType = newsletteritem,
                                PublicationCode = newsletterTypes.Where(x => x.Value.Equals(newsletteritem)).First().Key,
                                OptIn = true
                            });
                        }
                        Add(OptIns);
                    }
                }

                return UpdateNewsletterOptInsContext.Update(newsletterUserOptins);
            }
            return false;
        }


        public bool Add(IEnumerable<INewsletterUserOptIn> newsletterTypes)
        {
            return AddNewsletterUserOptInsContext.Add(newsletterTypes);
        }
    }
}
