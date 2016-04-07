using Informa.Library.Newsletter;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateSiteNewsletterUserOptIn : IUpdateSiteNewsletterUserOptIn
	{
		protected readonly INewsletterUserOptInFactory OptInFactory;
		protected readonly IUpdateNewsletterUserOptIns UpdateOptIns;
		protected readonly ISiteNewsletterTypeContext NewsletterTypeContext;

		public UpdateSiteNewsletterUserOptIn(
			INewsletterUserOptInFactory optInFactory,
			IUpdateNewsletterUserOptIns updateOptIns,
			ISiteNewsletterTypeContext newsletterTypeContext)
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

			var userOptIn = OptInFactory.Create(NewsletterTypeContext.Type, optIn);

			return UpdateOptIns.Update(new List<INewsletterUserOptIn>() { { userOptIn } }, username);
		}
	}
}
