using Informa.Library.Newsletter;
using System.Collections.Generic;

namespace Informa.Library.Site.Newsletter
{
	public interface ISiteNewsletterTypesContext
	{
		IEnumerable<NewsletterType> NewsletterTypes { get; }
	}
}
