using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public class GlassRenderingItemContext<T> : IRenderingItemContext<T>
		where T : class, IGlassBase
	{
		protected readonly ISitecoreService SitecoreService;
		protected readonly IRenderingContext RenderingContext;

		public GlassRenderingItemContext(
			ISitecoreService sitecoreService,
			IRenderingContext renderingContext)
		{
			SitecoreService = sitecoreService;
			RenderingContext = renderingContext;
		}

		public T Item => SitecoreService.GetItem<T>(RenderingContext.GetDataSource());
	}
}
