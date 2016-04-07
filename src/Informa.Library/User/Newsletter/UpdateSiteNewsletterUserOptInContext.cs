using Informa.Library.Newsletter;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateSiteNewsletterUserOptInContext : IUpdateSiteNewsletterUserOptInContext
	{
		protected readonly INewsletterUserOptInFactory OptInFactory;
		protected readonly IUpdateNewsletterUserOptInsContext UpdateOptIns;
		protected readonly ISiteNewsletterTypeContext NewsletterTypeContext;

		public UpdateSiteNewsletterUserOptInContext(
			INewsletterUserOptInFactory optInFactory,
			IUpdateNewsletterUserOptInsContext updateOptIns,
			ISiteNewsletterTypeContext newsletterTypeContext)
		{
			OptInFactory = optInFactory;
			UpdateOptIns = updateOptIns;
			NewsletterTypeContext = newsletterTypeContext;
		}

		public bool Update(bool optIn)
		{
			var userOptIn = OptInFactory.Create(NewsletterTypeContext.Type, optIn);

			return UpdateOptIns.Update(new List<INewsletterUserOptIn>() { { userOptIn } });
		}
	}
}
