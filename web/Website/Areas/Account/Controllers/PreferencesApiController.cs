using System.Web.Http;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.User.Newsletter;
using Informa.Web.Areas.Account.Models.User.Management;
using Informa.Library.Utilities.WebApi.Filters;

namespace Informa.Web.Areas.Account.Controllers
{
	public class PreferencesApiController : ApiController
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUpdateOfferUserOptIn OffersOptIn;
		protected readonly IUpdateSiteNewsletterUserOptIn UpdateSiteNewsletterOptIn;
		protected readonly IUpdateSiteNewsletterUserOptInContext UpdateSiteNewsletterOptInContext;
		protected readonly ISiteNewsletterUserOptedInContext NewsletterOptedInContext;

		public PreferencesApiController(
			IAuthenticatedUserContext userContext,
			IUpdateOfferUserOptIn offersOptIn,
			IUpdateSiteNewsletterUserOptIn updateSiteNewsletterOptIn,
			IUpdateSiteNewsletterUserOptInContext updateSiteNewsletterOptInContext,
			ISiteNewsletterUserOptedInContext newsletterOptedInContext)
		{
			UserContext = userContext;
			OffersOptIn = offersOptIn;
			UpdateSiteNewsletterOptIn = updateSiteNewsletterOptIn;
			UpdateSiteNewsletterOptInContext = updateSiteNewsletterOptInContext;
			NewsletterOptedInContext = newsletterOptedInContext;
		}

		[HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult Update(PreferencesRequest request)
		{
			var nResp = UpdateSiteNewsletterOptInContext.Update(request.NewsletterOptIn);
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