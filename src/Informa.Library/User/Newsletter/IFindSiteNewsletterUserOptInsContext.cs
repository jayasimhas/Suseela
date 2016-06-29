using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface IFindSiteNewsletterUserOptInsContext
	{
		IEnumerable<INewsletterUserOptIn> Find(ISiteNewsletterTypes newsletterTypes);
	}
}