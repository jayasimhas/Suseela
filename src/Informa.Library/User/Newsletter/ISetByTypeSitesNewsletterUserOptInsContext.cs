using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISetByTypeSitesNewsletterUserOptInsContext
	{
		bool Set(IEnumerable<string> newsletterTypes);
	}
}