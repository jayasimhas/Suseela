using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SiteNewsletterTypeContext : ISiteNewsletterTypeContext
	{
		public SiteNewsletterTypeContext(
			)
		{

		}

		public NewsletterType Type => NewsletterType.Scrip;
	}
}
