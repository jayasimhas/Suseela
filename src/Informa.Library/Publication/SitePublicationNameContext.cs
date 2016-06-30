using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitePublicationNameContext : ISitePublicationNameContext
	{
		protected ISiteRootContext SiteRootContext;
		protected readonly ISitePublicationNameFactory SitePublicationNameFactory;

		public SitePublicationNameContext(
			ISiteRootContext siteRootContext,
			ISitePublicationNameFactory sitePublicationNameFactory)
		{
			SiteRootContext = siteRootContext;
			SitePublicationNameFactory = sitePublicationNameFactory;
		}

		public string Name => SitePublicationNameFactory.Create(SiteRootContext.Item);
	}
}
