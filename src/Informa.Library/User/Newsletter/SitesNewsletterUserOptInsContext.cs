using Informa.Library.Newsletter;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SitesNewsletterUserOptInsContext : ISitesNewsletterUserOptInsContext
	{
		protected readonly ISitesNewsletterTypeContext SitesNewsletterTypeContext;
		protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;

		public SitesNewsletterUserOptInsContext(
			ISitesNewsletterTypeContext sitesNewsletterTypeContext,
			INewsletterUserOptInsContext newsletterUserOptInsContext)
		{
			SitesNewsletterTypeContext = sitesNewsletterTypeContext;
			NewsletterUserOptInsContext = newsletterUserOptInsContext;
		}

		public IEnumerable<INewsletterUserOptIn> OptIns => NewsletterUserOptInsContext.OptIns.Where(noi => SitesNewsletterTypeContext.Types.Any(t => noi.NewsletterType.StartsWith(t, System.StringComparison.InvariantCultureIgnoreCase)));
	}
}
