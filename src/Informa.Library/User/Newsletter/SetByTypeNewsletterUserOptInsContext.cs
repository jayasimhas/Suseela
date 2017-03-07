using Jabberwocky.Autofac.Attributes;
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
        public SetByTypeNewsletterUserOptInsContext(
            INewsletterUserOptInsContext newsletterUserOptInsContext,
            IUpdateNewsletterUserOptInsContext updateNewsletterOptInsContext,
            IAddNewsletterUserOptInsContext addNewsletterUserOptInsContext)
        {
            NewsletterUserOptInsContext = newsletterUserOptInsContext;
            UpdateNewsletterOptInsContext = updateNewsletterOptInsContext;
            AddNewsletterUserOptInsContext = addNewsletterUserOptInsContext;

        }

        public bool Set(IEnumerable<string> newsletterTypes)
        {
            var newsletterUserOptins = NewsletterUserOptInsContext.OptIns.ToList();

            newsletterUserOptins.ForEach(noi => noi.OptIn = newsletterTypes.Contains(noi.NewsletterType));

            return UpdateNewsletterOptInsContext.Update(newsletterUserOptins);
        }


        public bool Add(IEnumerable<INewsletterUserOptIn> newsletterTypes)
        {
            return AddNewsletterUserOptInsContext.Add(newsletterTypes);
        }
    }
}
