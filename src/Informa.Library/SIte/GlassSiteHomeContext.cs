using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class GlassSiteHomeContext : ISiteHomeContext
	{
		protected readonly ISitecoreContext SitecoreContext;

		public GlassSiteHomeContext(
			ISitecoreContext sitecoreContext)
		{
			SitecoreContext = sitecoreContext;
		}

		public IHome_Page Item => SitecoreContext.GetHomeItem<IHome_Page>();
	}
}
