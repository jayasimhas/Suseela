using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SiteNewsletterTypesContext : ISiteNewsletterTypesContext
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ISiteNewsletterTypesFactory SiteNewsletterTypesFactory;

		public SiteNewsletterTypesContext(
			ISiteRootContext siteRootContext,
			ISiteNewsletterTypesFactory siteNewsletterTypesFactory)
		{
			SiteRootContext = siteRootContext;
			SiteNewsletterTypesFactory = siteNewsletterTypesFactory;
		}

		private ISiteNewsletterTypes _types;
		public ISiteNewsletterTypes Types => _types ?? (_types = SiteNewsletterTypesFactory.Create(SiteRootContext.Item));
	}
}
