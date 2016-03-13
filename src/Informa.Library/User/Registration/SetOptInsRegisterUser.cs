using Informa.Library.Site.Newsletter;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Registration
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SetOptInsRegisterUser : ISetOptInsRegisterUser
	{
		protected readonly IUpdateOfferUserOptIn UpdateOfferUserOptIn;
		protected readonly IUpdateNewsletterUserOptIn UpdateNewsletterUserOptIn;
		protected readonly INewsletterUserOptInFactory NewsletterUserOptInFactory;
		protected readonly ISiteNewsletterTypesContext NewsletterTypesContext;

		public SetOptInsRegisterUser(
			IUpdateOfferUserOptIn updateOfferUserOptIn,
			IUpdateNewsletterUserOptIn updateNewsletterUserOptIn,
			INewsletterUserOptInFactory newsletterUserOptInFactory,
			ISiteNewsletterTypesContext newsletterTypesContext)
		{
			UpdateOfferUserOptIn = updateOfferUserOptIn;
			UpdateNewsletterUserOptIn = updateNewsletterUserOptIn;
			NewsletterUserOptInFactory = newsletterUserOptInFactory;
			NewsletterTypesContext = newsletterTypesContext;
		}

		public bool Set(INewUser newUser, bool offers, bool newsletters)
		{
			var userNewsletterOptIns = NewsletterTypesContext.NewsletterTypes.Select(nt => NewsletterUserOptInFactory.Create(nt, newsletters));
			var newsletterSucccess = UpdateNewsletterUserOptIn.Update(userNewsletterOptIns, newUser.Username);
			var offerSuccess = UpdateOfferUserOptIn.Update(newUser, offers);

			return offerSuccess && newsletterSucccess;
		}
	}
}
