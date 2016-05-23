using Jabberwocky.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterUserOptedInContext : ISiteNewsletterUserOptedInContext
	{
		protected readonly ISiteNewsletterUserOptInsContext SiteNewsletterUserOptInsContext;

		public SiteNewsletterUserOptedInContext(
			ISiteNewsletterUserOptInsContext siteNewsletterUserOptInsContext)
		{
			SiteNewsletterUserOptInsContext = siteNewsletterUserOptInsContext;
		}

		public bool OptedIn => SiteNewsletterUserOptInsContext.OptIns.Any(oi => oi.OptIn);
	}
}
