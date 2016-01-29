using System;
using System.Collections.Generic;

namespace Informa.Library.ContentCuration
{
	public interface IItemManuallyCuratedContent
	{
		IEnumerable<Guid> Get(Guid itemId);
	}
}
