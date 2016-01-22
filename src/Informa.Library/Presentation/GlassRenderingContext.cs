using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class GlassRenderingContext : RenderingContextMvcWrapper, IRenderingContext
	{

	}
}
