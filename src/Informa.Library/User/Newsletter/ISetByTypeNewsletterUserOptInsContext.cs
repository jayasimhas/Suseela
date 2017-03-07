using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISetByTypeNewsletterUserOptInsContext
	{
		bool Set(IEnumerable<string> newsletterTypes);
        bool Add(IEnumerable<INewsletterUserOptIn> newsletterTypes);

    }
}