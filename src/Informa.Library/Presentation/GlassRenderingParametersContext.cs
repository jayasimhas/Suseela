using System.Linq;
using Jabberwocky.Glass.Models;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class GlassRenderingParametersContext : IRenderingParametersContext
	{
		protected readonly ISitecoreContext SitecoreContext;

		public GlassRenderingParametersContext(
			ISitecoreContext sitecoreContext)
		{
			SitecoreContext = sitecoreContext;
		}

		public T GetParameters<T>() where T : class, IGlassBase
		{
			return new GlassHtml(SitecoreContext).GetRenderingParameters<T>(Parameters);
		}

		public string Parameters => string.Join("&", RenderingContext.Current.Rendering.Parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
	}
}