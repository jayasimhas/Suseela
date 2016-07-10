using Glass.Mapper.Sc.Web.Mvc;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Mvc.Presentation;
using System;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.PerScope)]
	public class GlassRenderingContext : RenderingContextMvcWrapper, IRenderingContext
	{
		public Guid Id => RenderingContext.Current.Rendering.Id;
	}
}
