using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SiteNewsletterTypeContext : ISiteNewsletterTypeContext
	{
		private ISiteRootContext SiteRootContext;
		public SiteNewsletterTypeContext(
						ISiteRootContext siteRootContext
			)
		{
			SiteRootContext = siteRootContext;
		}

		public string Type => SiteRootContext?.Item?.Publication_Name ?? string.Empty;
	}
}
