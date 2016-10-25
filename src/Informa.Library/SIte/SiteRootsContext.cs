using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Caching;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Models;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.Site
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SiteRootsContext : ISiteRootsContext
    {
        private static readonly string cacheKey = nameof(SiteRootsContext);
        protected readonly ICrossSiteCacheProvider CacheProvider;
        protected readonly ISitecoreService SitecoreService;
        protected readonly IVerticalRootContext VerticalrootContext;
        

        public SiteRootsContext(
            ISitecoreService sitecoreService,
            ICrossSiteCacheProvider cacheProvider,
            IVerticalRootContext verticalrootContext
            )
        {
            SitecoreService = sitecoreService;
            CacheProvider = cacheProvider;
            VerticalrootContext = verticalrootContext;
        }

        public IEnumerable<ISite_Root> SiteRoots => CacheProvider.GetFromCache($"cacheKey-{VerticalrootContext.Item?.Vertical_Name}", BuildSiteRootsContext);
        private IList<ISite_Root> BuildSiteRootsContext()
        {
            //JIRA Ticket IPMP-269            
            IEnumerable<ISite_Root> siteRoots = null;
            var vertical = VerticalrootContext.Item;           
            var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");
            foreach (var verticalContentItems in contentItem._ChildrenWithInferType.OfType<IVertical_Root>())
            {
                if (string.Equals(vertical.Vertical_Name, verticalContentItems.Vertical_Name,System.StringComparison.OrdinalIgnoreCase))
                    siteRoots = verticalContentItems._ChildrenWithInferType.OfType<ISite_Root>();                
            }
            return siteRoots.ToList();
        }
    }
}