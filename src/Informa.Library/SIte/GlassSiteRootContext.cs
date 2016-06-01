using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassSiteRootContext : ISiteRootContext
	{
		protected readonly ISiteRootsContext SiteRootsContext;

		public GlassSiteRootContext(ISitecoreContext sitecoreContext)
		{
			Item = sitecoreContext.GetRootItem<ISite_Root>();
		}
		public ISite_Root Item { get; set; }
	}
}
