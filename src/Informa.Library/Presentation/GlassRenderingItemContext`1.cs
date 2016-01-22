using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public class GlassRenderingItemContext<T> : IRenderingItemContext<T>
		where T : class, IGlassBase
	{
		protected readonly IRenderingItemContext RenderingItemContext;

		public GlassRenderingItemContext(
			IRenderingItemContext renderingItemContext)
		{
			RenderingItemContext = renderingItemContext;
		}

		public T Item => RenderingItemContext.Get<T>();
	}
}
