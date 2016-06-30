using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface ISitesNewsletterTypes
	{
		IEnumerable<ISiteNewsletterTypes> SiteTypes { get; }
	}
}