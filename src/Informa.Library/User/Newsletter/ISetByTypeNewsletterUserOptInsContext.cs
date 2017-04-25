using System.Collections.Generic;
using System.Collections.Specialized;

namespace Informa.Library.User.Newsletter
{
	public interface ISetByTypeNewsletterUserOptInsContext
	{
		bool Set(List<KeyValuePair<string, string>> newsletterTypes);
        bool Add(IEnumerable<INewsletterUserOptIn> newsletterTypes);

    }
}