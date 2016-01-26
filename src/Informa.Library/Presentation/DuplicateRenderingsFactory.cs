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
						RenderingItemId = new Guid("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}")
					}
				}
			};
		}
	}
}
