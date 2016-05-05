using Informa.Library.Publication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SiteNewsletterUserOptedInContext : ISiteNewsletterUserOptedInContext
	{
		protected readonly ISitePublicationContext NewsletterTypeContext;
		protected readonly INewsletterUserOptInsContext OptInsContext;

		public SiteNewsletterUserOptedInContext(
			ISitePublicationContext newsletterTypeContext,
			INewsletterUserOptInsContext optInsContext)
		{
			NewsletterTypeContext = newsletterTypeContext;
			OptInsContext = optInsContext;
		}

		public bool OptedIn => OptInsContext.OptIns.Any(oi => oi.NewsletterType.ToLower() == NewsletterTypeContext.Name.ToLower() && oi.OptIn);
	}
}
