using Glass.Mapper.Sc;
using Informa.Library.Services.Global;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassRenderingItemContext : IRenderingItemContext
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IRenderingContext RenderingContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public GlassRenderingItemContext(
			ISitecoreContext sitecoreContext,
			IRenderingContext renderingContext,
            IGlobalSitecoreService globalService)
		{
			SitecoreContext = sitecoreContext;
			RenderingContext = renderingContext;
            GlobalService = globalService;

		}

		public T Get<T>()
			where T : class, IGlassBase
		{
			return RenderingContext.HasDataSource ? GlobalService.GetItem<T>(RenderingContext.GetDataSource()) : SitecoreContext.GetCurrentItem<T>();
		}
	}
}
