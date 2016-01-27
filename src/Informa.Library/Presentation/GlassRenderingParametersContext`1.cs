using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public class GlassRenderingParametersContext<T> : IRenderingParametersContext<T>
		where T : class, IGlassBase
	{
		protected readonly IRenderingParametersContext Context;

		public GlassRenderingParametersContext(
			IRenderingParametersContext renderingParametersContext)
		{
			Context = renderingParametersContext;
		}

		public T Parameters => Context.GetParameters<T>();
	}
}
