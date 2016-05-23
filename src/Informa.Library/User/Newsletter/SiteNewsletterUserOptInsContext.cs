using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterUserOptInsContext : ISiteNewsletterUserOptInsContext
	{
		private readonly IFindSiteNewsletterUserOptInsContext FindSiteNewsletterUserOptInsContext;
		protected readonly ISiteNewsletterTypesContext SiteNewsletterTypesContext;

		public SiteNewsletterUserOptInsContext(
			IFindSiteNewsletterUserOptInsContext findSiteNewsletterUserOptInsContext,
			ISiteNewsletterTypesContext siteNewsletterTypesContext)
		{
			FindSiteNewsletterUserOptInsContext = findSiteNewsletterUserOptInsContext;
			SiteNewsletterTypesContext = siteNewsletterTypesContext;
		}

		public IEnumerable<INewsletterUserOptIn> OptIns => FindSiteNewsletterUserOptInsContext.Find(SiteNewsletterTypesContext.Types);
	}
}
