using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Informa.Library.Presentation
{
	public interface IDefaultItemRenderingsFactory
	{
		IEnumerable<IItemRendering> Get(Item item);
	}
}
