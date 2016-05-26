using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterUserOptedInContext : ISiteNewsletterUserOptedInContext
	{
		protected readonly ISiteNewsletterUserOptInsContext SiteNewsletterUserOptInsContext;
		protected readonly ISiteNewsletterUserOptedIn SiteNewsletterUserOptedIn;

		public SiteNewsletterUserOptedInContext(
			ISiteNewsletterUserOptInsContext siteNewsletterUserOptInsContext,
			ISiteNewsletterUserOptedIn siteNewsletterUserOptedIn)
		{
			SiteNewsletterUserOptInsContext = siteNewsletterUserOptInsContext;
			SiteNewsletterUserOptedIn = siteNewsletterUserOptedIn;
		}

		public bool OptedIn => SiteNewsletterUserOptedIn.Check(SiteNewsletterUserOptInsContext.OptIns);
	}
}
