using Informa.Library.Site;
using Informa.Library.Threading;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.PerRequest)]
	public class SitePublicationsContext : ThreadSafe<IEnumerable<ISitePublication>>, ISitePublicationsContext
	{
		protected readonly ISiteRootsContext SiteRootsContext;
		private readonly ISitePublicationFactory PublicationFactory;

		public SitePublicationsContext(
			ISiteRootsContext siteRootsContext,
			ISitePublicationFactory publicationFactory)
		{
			SiteRootsContext = siteRootsContext;
			PublicationFactory = publicationFactory;
		}

		public IEnumerable<ISitePublication> Publications => SafeObject;

		protected override IEnumerable<ISitePublication> UnsafeObject => SiteRootsContext.SiteRoots.Select(sr => PublicationFactory.Create(sr));
	}
}
