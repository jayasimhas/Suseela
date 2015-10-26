using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Library.Utilities.References
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class RenderingReferences : IRenderingReferences
	{
		public static IRenderingReferences Instance
		{
			get { return AutofacConfig.ServiceLocator.Resolve<Owned<IRenderingReferences>>().Value; }
		}
		
	}
}
