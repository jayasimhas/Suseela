using Glass.Mapper.Sc;
using Informa.Library.Threading;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Core.Caching;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteRootsContext : ThreadSafe<IEnumerable<ISite_Root>>, ISiteRootsContext
	{
		protected readonly ISitecoreService SitecoreService;
	    protected readonly ICacheProvider CacheProvider;
	    private static readonly string cacheKey = nameof(SiteRootsContext);

		public SiteRootsContext(
			ISitecoreService sitecoreService,
            ICacheProvider cacheProvider)
		{
			SitecoreService = sitecoreService;
		    CacheProvider = cacheProvider;

		}

		public IEnumerable<ISite_Root> SiteRoots => SafeObject;

		protected override IEnumerable<ISite_Root> UnsafeObject => CacheProvider.GetFromCache(cacheKey, BuildSiteRootsContext);

		private IEnumerable<ISite_Root> BuildSiteRootsContext()
	    {
            var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");

            var siteRoots = contentItem._ChildrenWithInferType.OfType<ISite_Root>();
            
            return siteRoots;
        }
	}
}
