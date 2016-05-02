using System.Collections.Generic;

namespace Informa.Library.Newsletter
{
	public interface ISitesNewsletterTypeContext
	{
		IEnumerable<string> Types { get; }
	}
}