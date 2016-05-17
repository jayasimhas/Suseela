using Informa.Library.Publication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateSiteNewsletterUserOptIn : IUpdateSiteNewsletterUserOptIn
	{
		protected readonly INewsletterUserOptInFactory OptInFactory;
		protected readonly IUpdateNewsletterUserOptIns UpdateOptIns;
		protected readonly ISitePublicationContext NewsletterTypeContext;

		public UpdateSiteNewsletterUserOptIn(
			INewsletterUserOptInFactory optInFactory,
			IUpdateNewsletterUserOptIns updateOptIns,
			ISitePublicationContext newsletterTypeContext)
		{
			OptInFactory = optInFactory;
			UpdateOptIns = updateOptIns;
			NewsletterTypeContext = newsletterTypeContext;
		}

		public bool Update(string username, bool optIn)
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				return false;
			}

			var userOptIn = OptInFactory.Create(NewsletterTypeContext.Name, optIn);

			return UpdateOptIns.Update(new List<INewsletterUserOptIn>() { { userOptIn } }, username);
		}
	}
}
