using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Caching;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteRootsContext : ISiteRootsContext
	{
		private static readonly string cacheKey = nameof(SiteRootsContext);
		protected readonly ICrossSiteCacheProvider CacheProvider;
		protected readonly ISitecoreService SitecoreService;

		public SiteRootsContext(
			ISitecoreService sitecoreService,
			ICrossSiteCacheProvider cacheProvider)
		{
			SitecoreService = sitecoreService;
			CacheProvider = cacheProvider;
		}

		public IEnumerable<ISite_Root> SiteRoots => CacheProvider.GetFromCache(cacheKey, BuildSiteRootsContext);

		private IList<ISite_Root> BuildSiteRootsContext()
		{
            //JIRA Ticket IPMP-269

            IEnumerable<ISite_Root> siteRoots = null;
            var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");
            foreach (var verticalContentItems in contentItem._ChildrenWithInferType)
            {
                siteRoots = verticalContentItems._ChildrenWithInferType.OfType<ISite_Root>();
            }

            return siteRoots.ToList();
        }
    }
}