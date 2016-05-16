using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.Default)]
	public class SitesPublicationContext : ISitesPublicationContext
	{
		protected readonly ISiteRootsContext SiteRootsContext;

		public SitesPublicationContext(
			ISiteRootsContext siteRootsContext)
		{
			SiteRootsContext = siteRootsContext;
		}

		public IEnumerable<string> Names => SiteRootsContext.SiteRoots.Select(sr => sr.Publication_Name).ToList();
	}
}
