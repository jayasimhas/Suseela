using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Glass.Mapper.Sc;
using Sitecore;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassSiteRootContext : ISiteRootContext
	{
		protected readonly ISitecoreService SitecoreService;

		public GlassSiteRootContext(ISitecoreService sitecoreService)
		{
			SitecoreService = sitecoreService;
		}

		public ISite_Root Item => SitecoreService.GetItem<ISite_Root>(Context.Site.RootPath);
	}
}
