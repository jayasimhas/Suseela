using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class DuplicateRenderingsFactory : IDuplicateRenderingsFactory
	{
		public IEnumerable<IRendering> Create()
		{
			return new List<Rendering>
			{
				{
					new Rendering
					{
						RenderingItemId = new Guid("{C7E595D3-9F0C-41E0-890F-6D8C54B8EF40}")
					}
				}
			};
		}
	}
}
