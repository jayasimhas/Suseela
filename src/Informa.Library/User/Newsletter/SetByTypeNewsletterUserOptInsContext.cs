using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class SetByTypeNewsletterUserOptInsContext : ISetByTypeNewsletterUserOptInsContext
	{
		protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;
		protected readonly IUpdateNewsletterUserOptInsContext UpdateNewsletterOptInsContext;

		public SetByTypeNewsletterUserOptInsContext(
			INewsletterUserOptInsContext newsletterUserOptInsContext,
			IUpdateNewsletterUserOptInsContext updateNewsletterOptInsContext)
		{
			NewsletterUserOptInsContext = newsletterUserOptInsContext;
			UpdateNewsletterOptInsContext = updateNewsletterOptInsContext;
		}

		public bool Set(IEnumerable<string> newsletterTypes)
		{
			var newsletterUserOptins = NewsletterUserOptInsContext.OptIns.ToList();

			newsletterUserOptins.ForEach(noi => noi.OptIn = newsletterTypes.Contains(noi.NewsletterType));

			return UpdateNewsletterOptInsContext.Update(newsletterUserOptins);
		}
	}
}
