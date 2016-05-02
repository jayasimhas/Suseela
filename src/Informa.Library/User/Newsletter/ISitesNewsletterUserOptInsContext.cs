using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISitesNewsletterUserOptInsContext
	{
		IEnumerable<INewsletterUserOptIn> OptIns { get; }
	}
}