using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterUserOptInsContext : ISiteNewsletterUserOptInsContext
	{
		protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;
		protected readonly ISiteNewsletterTypesContext SiteNewsletterTypesContext;

		public SiteNewsletterUserOptInsContext(
			INewsletterUserOptInsContext newsletterUserOptInsContext,
			ISiteNewsletterTypesContext siteNewsletterTypesContext)
		{
			NewsletterUserOptInsContext = newsletterUserOptInsContext;
			SiteNewsletterTypesContext = siteNewsletterTypesContext;
		}

		public IEnumerable<INewsletterUserOptIn> OptIns => NewsletterUserOptInsContext.OptIns.Where(noi => IsSiteMatch(noi.NewsletterType));

		public bool IsSiteMatch(string newsletterType)
		{
			var types = SiteNewsletterTypesContext.Types;

			return
				IsMatch(newsletterType, types.Breaking) ||
				IsMatch(newsletterType, types.Daily) ||
				IsMatch(newsletterType, types.Breaking);
		}

		public bool IsMatch(string newsletterType1, string newsletterType2)
		{
			return string.Equals(newsletterType1, newsletterType2, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
