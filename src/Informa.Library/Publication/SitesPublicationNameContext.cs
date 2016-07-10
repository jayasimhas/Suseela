using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService]
	public class SitesPublicationNameContext : ISitesPublicationNameContext
	{
		protected readonly ISitePublicationsContext SitePublicationsContext;

		public SitesPublicationNameContext(
			ISitePublicationsContext sitePublicationsContext)
		{
			SitePublicationsContext = sitePublicationsContext;
		}

		public IEnumerable<string> Names => SitePublicationsContext.Publications.Select(p => p.Name);
	}
}
