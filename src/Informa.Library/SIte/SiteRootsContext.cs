using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
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
		protected readonly ICacheProvider CacheProvider;
		protected readonly ISitecoreService SitecoreService;

		public SiteRootsContext(
			ISitecoreService sitecoreService,
			ICacheProvider cacheProvider)
		{
			SitecoreService = sitecoreService;
			CacheProvider = cacheProvider;
		}

		public IEnumerable<ISite_Root> SiteRoots => CacheProvider.GetFromCache(cacheKey, BuildSiteRootsContext);

		private IList<ISite_Root> BuildSiteRootsContext()
		{
			var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");
			var siteRoots = contentItem._ChildrenWithInferType.OfType<ISite_Root>();
			return siteRoots.ToList();
		}
	}
}