using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.Default)]
	public class SitePublicationContext : ISitePublicationContext
	{
		private ISiteRootContext SiteRootContext;
		public SitePublicationContext(
						ISiteRootContext siteRootContext
			)
		{
			SiteRootContext = siteRootContext;
		}

		public string Name => SiteRootContext?.Item?.Publication_Name ?? string.Empty;
	}
}
