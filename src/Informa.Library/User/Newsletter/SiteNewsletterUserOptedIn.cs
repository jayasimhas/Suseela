using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterUserOptedIn : ISiteNewsletterUserOptedIn
	{
		public bool Check(IEnumerable<INewsletterUserOptIn> optIns)
		{
			return optIns.Any(oi => oi.OptIn);
		}
	}
}
