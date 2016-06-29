using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitePublicationNameContext : ISitePublicationNameContext
	{
		protected ISiteRootContext SiteRootContext;
		protected readonly ISitePublicationCodeFactory SitePublicationNameFactory;

		public SitePublicationNameContext(
			ISiteRootContext siteRootContext,
			ISitePublicationCodeFactory sitePublicationNameFactory)
		{
			SiteRootContext = siteRootContext;
			SitePublicationNameFactory = sitePublicationNameFactory;
		}

		public string Name => SitePublicationNameFactory.Create(SiteRootContext.Item);
	}
}
