using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Newsletter
{
	public class SiteNewsletterTypeContext : ISiteNewsletterTypeContext
	{
		[AutowireService(LifetimeScope.Default)]
		public SiteNewsletterTypeContext(
			)
		{

		}

		public NewsletterType Type => NewsletterType.Scrip;
	}
}
