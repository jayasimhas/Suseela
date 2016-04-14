using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassRenderingItemContext : IRenderingItemContext
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IRenderingContext RenderingContext;

		public GlassRenderingItemContext(
			ISitecoreContext sitecoreContext,
			IRenderingContext renderingContext)
		{
			SitecoreContext = sitecoreContext;
			RenderingContext = renderingContext;
		}

		public T Get<T>()
			where T : class, IGlassBase
		{
			return RenderingContext.HasDataSource ? SitecoreContext.GetItem<T>(RenderingContext.GetDataSource()) : SitecoreContext.GetCurrentItem<T>();
		}
	}
}
