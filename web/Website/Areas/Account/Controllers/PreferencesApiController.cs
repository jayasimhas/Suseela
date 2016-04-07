using System.Collections.Generic;
using System.Web.Http;
using Informa.Library.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Web.Areas.Account.Models.User.Management;
using Informa.Library.Utilities.WebApi.Filters;

namespace Informa.Web.Areas.Account.Controllers
{
	public class PreferencesApiController : ApiController
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUpdateNewsletterUserOptIns NewsletterOptIn;
		protected readonly INewsletterUserOptInFactory NewsletterUserOptInFactory;
		protected readonly IUpdateOfferUserOptIn OffersOptIn;
		protected readonly ISiteNewsletterUserOptedInContext NewsletterOptedInContext;

		public PreferencesApiController(
			IAuthenticatedUserContext userContext,
			IUpdateNewsletterUserOptIns newsletterOptIn,
			INewsletterUserOptInFactory newsletterUserOptInFactory,
			IUpdateOfferUserOptIn offersOptIn,
			ISiteNewsletterUserOptedInContext newsletterOptedInContext)
		{
			UserContext = userContext;
			NewsletterOptIn = newsletterOptIn;
			NewsletterUserOptInFactory = newsletterUserOptInFactory;
			OffersOptIn = offersOptIn;
			NewsletterOptedInContext = newsletterOptedInContext;
		}

		[HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult Update(PreferencesRequest request)
		{
			var userNewsletterOptIns = new List<INewsletterUserOptIn>() { NewsletterUserOptInFactory.Create(NewsletterType.Scrip, request.NewsletterOptIn) };
			var nResp = NewsletterOptIn.Update(userNewsletterOptIns, UserContext.User.Username);
			var oResp = OffersOptIn.Update(UserContext.User?.Username, !request.DoNotSendOffersOptIn);

			var success = nResp && oResp;

			return Ok(new
			{
				success = success
			});
		}


		[HttpGet]
		public bool SignupUser(string userName)
		{
			var userNewsletterOptIns = new List<INewsletterUserOptIn>() { NewsletterUserOptInFactory.Create(NewsletterType.Scrip,true) };
			var nResp = NewsletterOptIn.Update(userNewsletterOptIns, userName);
			return nResp;
		}

		[HttpGet]
		public bool IsUserSignedUp()
		{
			return NewsletterOptedInContext.OptedIn;
		}
	}
}