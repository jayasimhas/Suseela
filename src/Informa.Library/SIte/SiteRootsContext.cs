using Glass.Mapper.Sc;
using Informa.Library.Threading;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerRequest)]
	public class SiteRootsContext : ThreadSafe<IEnumerable<ISite_Root>>, ISiteRootsContext
	{
		protected readonly ISitecoreService SitecoreService;

		public SiteRootsContext(
			ISitecoreService sitecoreService)
		{
			SitecoreService = sitecoreService;
		}

		public IEnumerable<ISite_Root> SiteRoots => SafeObject;

		protected override IEnumerable<ISite_Root> UnsafeObject
		{
			get
			{
				var contentItem = SitecoreService.GetItem<IGlassBase>("/sitecore/content");

				return contentItem._ChildrenWithInferType.Where(sr => sr is ISite_Root).Cast<ISite_Root>();
			}
		}
	}
}
