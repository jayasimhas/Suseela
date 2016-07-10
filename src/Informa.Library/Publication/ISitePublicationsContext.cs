using System.Collections.Generic;

namespace Informa.Library.Publication
{
	public interface ISitePublicationsContext
	{
		IEnumerable<ISitePublication> Publications { get; }
	}
}