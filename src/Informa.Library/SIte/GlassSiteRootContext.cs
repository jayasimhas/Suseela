using System.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Glass.Mapper.Sc;
using Sitecore;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassSiteRootContext : ISiteRootContext
	{
		protected readonly ISiteRootsContext SiteRootsContext;

		public GlassSiteRootContext(ISiteRootsContext siteRootsContext)
		{
            SiteRootsContext = siteRootsContext;
		}
        public ISite_Root Item => SiteRootsContext.SiteRoots.FirstOrDefault(a => a._Path.Equals(Context.Site.RootPath));
	}
}
