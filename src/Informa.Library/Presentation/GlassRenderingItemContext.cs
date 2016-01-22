using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class GlassRenderingItemContext : IRenderingItemContext
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly ISitecoreService SitecoreService;
		protected readonly IRenderingContext RenderingContext;

		public GlassRenderingItemContext(
			ISitecoreContext sitecoreContext,
			ISitecoreService sitecoreService,
			IRenderingContext renderingContext)
		{
			SitecoreContext = sitecoreContext;
			SitecoreService = sitecoreService;
			RenderingContext = renderingContext;
		}

		public T Get<T>()
			where T : class, IGlassBase
		{
			return RenderingContext.HasDataSource ? SitecoreService.GetItem<T>(RenderingContext.GetDataSource()) : SitecoreContext.GetCurrentItem<T>();
		}
	}
}
