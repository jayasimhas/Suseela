using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitesPublicationContext : ISitesPublicationContext
	{
		protected readonly ISiteRootsContext SiteRootsContext;
		protected readonly ISitePublicationNameFactory SitePublicationNameFactory;

		public SitesPublicationContext(
			ISiteRootsContext siteRootsContext,
			ISitePublicationNameFactory sitePublicationNameFactory)
		{
			SiteRootsContext = siteRootsContext;
			SitePublicationNameFactory = sitePublicationNameFactory;
		}

		public IEnumerable<string> Names => SiteRootsContext.SiteRoots.Select(sr => SitePublicationNameFactory.Create(sr)).ToList();
	}
}
