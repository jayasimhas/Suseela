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
        protected readonly IVerticalRootContext VerticalRootContext;

        public SiteRootsContext(
            ISitecoreService sitecoreService,
            ICrossSiteCacheProvider cacheProvider,
            IVerticalRootContext verticalRootContext)
        {
            SitecoreService = sitecoreService;
            CacheProvider = cacheProvider;
            VerticalRootContext = verticalRootContext;
        }

        public IEnumerable<ISite_Root> SiteRoots => CacheProvider.GetFromCache(cacheKey, BuildSiteRootsContext); 
        private IList<ISite_Root> BuildSiteRootsContext()
        {
            var item=VerticalRootContext.Item;
            //JIRA Ticket IPMP-269            
            IEnumerable<ISite_Root> siteRoots = null;
            ItemIdResolver verticalResolver = new ItemIdResolver();
            var Item=verticalResolver.GetVerticalName();
            var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");
            foreach (var verticalContentItems in contentItem._ChildrenWithInferType.OfType<IVertical_Root>())
            {
                if (string.Equals(Item._Path.Split('/')[3], verticalContentItems.Vertical_Name,System.StringComparison.OrdinalIgnoreCase))
                    siteRoots = verticalContentItems._ChildrenWithInferType.OfType<ISite_Root>();                
            }
            return siteRoots.ToList();
        }
    }
}