using Informa.Library.Newsletter;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SetByTypeSitesNewsletterUserOptInsContext : ISetByTypeSitesNewsletterUserOptInsContext
	{
		protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;
		protected readonly IUpdateNewsletterUserOptInsContext UpdateNewsletterOptInsContext;
		protected readonly ISitesNewsletterTypeContext SitesNewsletterTypeContext;

		public SetByTypeSitesNewsletterUserOptInsContext(
			INewsletterUserOptInsContext newsletterUserOptInsContext,
			IUpdateNewsletterUserOptInsContext updateNewsletterOptInsContext,
			ISitesNewsletterTypeContext sitesNewsletterTypeContext)
		{
			NewsletterUserOptInsContext = newsletterUserOptInsContext;
			UpdateNewsletterOptInsContext = updateNewsletterOptInsContext;
			SitesNewsletterTypeContext = sitesNewsletterTypeContext;
		}

		public bool Set(IEnumerable<string> newsletterTypes)
		{
			var newsletterUserOptins = NewsletterUserOptInsContext.OptIns.ToList();

			newsletterUserOptins
				.Where(noi => SitesNewsletterTypeContext.Types.Contains(noi.NewsletterType))
				.ToList()
				.ForEach(noi => noi.OptIn = newsletterTypes.Contains(noi.NewsletterType));

			return UpdateNewsletterOptInsContext.Update(newsletterUserOptins);
		}
	}
}
