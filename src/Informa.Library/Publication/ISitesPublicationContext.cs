using System.Collections.Generic;

namespace Informa.Library.Publication
{
	public interface ISitesPublicationContext
	{
		IEnumerable<string> Names { get; }
	}
}