﻿using Informa.Library.User.Offer;
using Informa.Library.User.Newsletter;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SetOptInsRegisterUser : ISetOptInsRegisterUser
	{
		protected readonly IUpdateOfferUserOptIn UpdateOfferUserOptIn;
		protected readonly IUpdateSiteNewsletterUserOptIn UpdateNewsletterOptIns;

		public SetOptInsRegisterUser(
			IUpdateOfferUserOptIn updateOfferUserOptIn,
			IUpdateSiteNewsletterUserOptIn updateNewsletterOptIns)
		{
			UpdateOfferUserOptIn = updateOfferUserOptIn;
			UpdateNewsletterOptIns = updateNewsletterOptIns;
		}

		public bool Set(INewUser newUser, bool offers, bool newsletters)
		{
			var username = newUser?.Username;

		    return Set(username, offers, newsletters);
		}

        public bool Set(string username, bool offers, bool newsletters)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            var newsletterSucccess = UpdateNewsletterOptIns.Update(username, newsletters);
            var offerSuccess = UpdateOfferUserOptIn.Update(username, offers);

            return offerSuccess && newsletterSucccess;
        }
    }
}
