using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISiteNewsletterUserOptInsContext
	{
		IEnumerable<INewsletterUserOptIn> OptIns { get; }
	}
}