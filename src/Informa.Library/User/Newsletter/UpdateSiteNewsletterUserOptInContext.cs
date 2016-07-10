using Informa.Library.Publication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateSiteNewsletterUserOptInContext : IUpdateSiteNewsletterUserOptInContext
	{
		protected readonly INewsletterUserOptInFactory OptInFactory;
		protected readonly IUpdateNewsletterUserOptInsContext UpdateOptIns;
		protected readonly ISitePublicationNameContext NewsletterTypeContext;

		public UpdateSiteNewsletterUserOptInContext(
			INewsletterUserOptInFactory optInFactory,
			IUpdateNewsletterUserOptInsContext updateOptIns,
			ISitePublicationNameContext newsletterTypeContext)
		{
			OptInFactory = optInFactory;
			UpdateOptIns = updateOptIns;
			NewsletterTypeContext = newsletterTypeContext;
		}

		public bool Update(bool optIn)
		{
			var userOptIn = OptInFactory.Create(NewsletterTypeContext.Name, optIn);

			return UpdateOptIns.Update(new List<INewsletterUserOptIn>() { { userOptIn } });
		}
	}
}
