using System.Collections.Generic;

namespace Informa.Library.Publication
{
	public interface ISitesPublicationNameContext
	{
		IEnumerable<string> Names { get; }
	}
}