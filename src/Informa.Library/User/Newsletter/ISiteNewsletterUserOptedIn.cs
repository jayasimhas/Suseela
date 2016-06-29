using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISiteNewsletterUserOptedIn
	{
		bool Check(IEnumerable<INewsletterUserOptIn> optIns);
	}
}