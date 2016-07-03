using Jabberwocky.Glass.Models;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassRenderingParametersContext : IRenderingParametersContext
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IRenderingContext RenderingContext;

		public GlassRenderingParametersContext(
			ISitecoreContext sitecoreContext,
			IRenderingContext renderingContext)
		{
			SitecoreContext = sitecoreContext;
			RenderingContext = renderingContext;
		}

		public T GetParameters<T>() where T : class, IGlassBase
		{
			return new GlassHtml(SitecoreContext).GetRenderingParameters<T>(RenderingContext.GetRenderingParameters());
		}
	}
}