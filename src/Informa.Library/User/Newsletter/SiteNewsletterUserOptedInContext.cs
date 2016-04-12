using Informa.Library.Newsletter;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SiteNewsletterUserOptedInContext : ISiteNewsletterUserOptedInContext
	{
		protected readonly ISiteNewsletterTypeContext NewsletterTypeContext;
		protected readonly INewsletterUserOptInsContext OptInsContext;

		public SiteNewsletterUserOptedInContext(
			ISiteNewsletterTypeContext newsletterTypeContext,
			INewsletterUserOptInsContext optInsContext)
		{
			NewsletterTypeContext = newsletterTypeContext;
			OptInsContext = optInsContext;
		}

		public bool OptedIn => OptInsContext.OptIns.Any(oi => oi.NewsletterType == NewsletterTypeContext.Type && oi.OptIn);
	}
}
