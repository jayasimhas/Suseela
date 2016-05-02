﻿using System.Web.Http;
using Informa.Library.User.Offer;
using Informa.Library.User.Newsletter;
using Informa.Web.Areas.Account.Models.User.Management;
using Informa.Library.Utilities.WebApi.Filters;
using System.Linq;

namespace Informa.Web.Areas.Account.Controllers
{
	public class PreferencesApiController : ApiController
	{
		protected readonly IUpdateOfferUserOptInContext OffersOptIn;
		protected readonly IUpdateSiteNewsletterUserOptIn UpdateSiteNewsletterOptIn;
		protected readonly ISiteNewsletterUserOptedInContext NewsletterOptedInContext;
		protected readonly ISetByTypeSitesNewsletterUserOptInsContext SetNewsletterUserOptInsContext;

		public PreferencesApiController(
			IUpdateOfferUserOptInContext offersOptIn,
			IUpdateSiteNewsletterUserOptIn updateSiteNewsletterOptIn,
			ISiteNewsletterUserOptedInContext newsletterOptedInContext,
			ISetByTypeSitesNewsletterUserOptInsContext setNewsletterUserOptInsContext)
		{
			OffersOptIn = offersOptIn;
			UpdateSiteNewsletterOptIn = updateSiteNewsletterOptIn;
			NewsletterOptedInContext = newsletterOptedInContext;
			SetNewsletterUserOptInsContext = setNewsletterUserOptInsContext;
		}

		[HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult Update(PreferencesRequest request)
		{
			var newsletterUpdated = SetNewsletterUserOptInsContext.Set(request.NewsletterOptIns ?? Enumerable.Empty<string>());
			var offersUpdated = OffersOptIn.Update(!request.DoNotSendOffersOptIn);

			return Ok(new
			{
				success = newsletterUpdated && offersUpdated
			});
		}

		[HttpGet]
		public bool SignupUser(string userName)
		{
			var nResp = UpdateSiteNewsletterOptIn.Update(userName, true);

			return nResp;
		}

		[HttpGet]
		public bool IsUserSignedUp()
		{
			return NewsletterOptedInContext.OptedIn;
		}
	}
}