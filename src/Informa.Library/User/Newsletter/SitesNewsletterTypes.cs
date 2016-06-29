using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SitesNewsletterTypes : ISitesNewsletterTypes
	{
		protected readonly ISiteRootsContext SiteRootsContext;
		protected readonly ISiteNewsletterTypesFactory SiteNewsletterTypesFactory;

		public SitesNewsletterTypes(
			ISiteRootsContext siteRootsContext,
			ISiteNewsletterTypesFactory siteNewsletterTypesFactory)
		{
			SiteRootsContext = siteRootsContext;
			SiteNewsletterTypesFactory = siteNewsletterTypesFactory;
		}

		public IEnumerable<ISiteNewsletterTypes> SiteTypes => SiteRootsContext.SiteRoots.Select(sr => SiteNewsletterTypesFactory.Create(sr));
	}
}
