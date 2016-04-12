using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore;
using Sitecore.Layouts;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class DefaultItemRenderingsFactory : IDefaultItemRenderingsFactory
	{
		public IEnumerable<IItemRendering> Get(Item item)
		{
			var itemRenderings = new List<IItemRendering>();

			if (item == null)
			{
				return itemRenderings;
			}

			var device = Context.Database.Resources.Devices.GetAll().FirstOrDefault(d => string.Equals(d.Name, "default", StringComparison.InvariantCultureIgnoreCase));

			if (device == null)
			{
				return itemRenderings;
			}

			var renderings = item.Visualization.GetRenderings(device, false) ?? new RenderingReference[0];

			itemRenderings.AddRange(renderings.Select(r => new ItemRendering
			{
				Datasource = r?.Settings?.DataSource,
				RenderingItemId = r?.RenderingItem?.ID.Guid ?? Guid.Empty
			}));

			return itemRenderings;
		}
	}
}
