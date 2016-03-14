using System.Collections.Generic;
using System.Web.Http;
using Informa.Library.Newsletter;
using Informa.Library.Site.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Web.Areas.Account.Models.User.Management;
using Informa.Library.Utilities.WebApi.Filters;

namespace Informa.Web.Areas.Account.Controllers
{
	public class PreferencesApiController : ApiController
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUpdateNewsletterUserOptIn NewsletterOptIn;
		protected readonly INewsletterUserOptInFactory NewsletterUserOptInFactory;
		protected readonly IUpdateOfferUserOptIn OffersOptIn;

		public PreferencesApiController(
			IAuthenticatedUserContext userContext,
			IUpdateNewsletterUserOptIn newsletterOptIn,
			INewsletterUserOptInFactory newsletterUserOptInFactory,
			IUpdateOfferUserOptIn offersOptIn)
		{
			UserContext = userContext;
			NewsletterOptIn = newsletterOptIn;
			NewsletterUserOptInFactory = newsletterUserOptInFactory;
			OffersOptIn = offersOptIn;
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


		[HttpPost]
		[ArgumentsRequired]
		public IHttpActionResult SignupUser(string userName)
		{
			var userNewsletterOptIns = new List<INewsletterUserOptIn>() { NewsletterUserOptInFactory.Create(NewsletterType.Scrip,true) };
			var nResp = NewsletterOptIn.Update(userNewsletterOptIns, userName);
			var oResp = OffersOptIn.Update(userName,false);

			var success = nResp && oResp;

			return Ok(new
			{
				success = success
			});
		}

		[HttpGet]
		public bool IsUserSignedUp()
		{
			var user = UserContext.User;
			if (UserContext.IsAuthenticated)
			{
				return NewsletterOptIn.IsUserSignedUp(UserContext.User.Username);
			}
			return false;
		}
	}
}