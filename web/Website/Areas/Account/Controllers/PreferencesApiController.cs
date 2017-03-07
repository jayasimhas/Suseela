using System.Web.Http;
using Informa.Library.User.Offer;
using Informa.Library.User.Newsletter;
using Informa.Web.Areas.Account.Models.User.Management;
using Informa.Library.Utilities.WebApi.Filters;
using System.Linq;
using Informa.Library.User.Profile;

namespace Informa.Web.Areas.Account.Controllers
{
	public class PreferencesApiController : ApiController
	{
		protected readonly IUpdateOfferUserOptInContext OffersOptIn;
		protected readonly IUpdateSiteNewsletterUserOptIn UpdateSiteNewsletterOptIn;
		protected readonly ISiteNewsletterUserOptedInContext NewsletterOptedInContext;
		protected readonly ISetPublicationsNewsletterUserOptIns SetNewsletterUserOptInsContext;
		protected readonly IFindUserProfileByUsername FindUserProfile;

		public PreferencesApiController(
			IUpdateOfferUserOptInContext offersOptIn,
			IUpdateSiteNewsletterUserOptIn updateSiteNewsletterOptIn,
			ISiteNewsletterUserOptedInContext newsletterOptedInContext,
			ISetPublicationsNewsletterUserOptIns setNewsletterUserOptInsContext,
			IFindUserProfileByUsername findUserProfile)
		{
			OffersOptIn = offersOptIn;
			UpdateSiteNewsletterOptIn = updateSiteNewsletterOptIn;
			NewsletterOptedInContext = newsletterOptedInContext;
			SetNewsletterUserOptInsContext = setNewsletterUserOptInsContext;
			FindUserProfile = findUserProfile;
		}

		[HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult Update(PreferencesRequest request)
		{         
            var newsletterUpdated = SetNewsletterUserOptInsContext.Set(request.Publications ?? Enumerable.Empty<string>(), NewsletterPreference.Update);
			var offersUpdated = OffersOptIn.Update(!request.DoNotSendOffersOptIn, NewsletterPreference.Update);

			return Ok(new
			{
				success = newsletterUpdated && offersUpdated
			});
		}

		[HttpGet]
		public string SignupUser(string userName)
		{
			if (string.IsNullOrEmpty(userName))
				return "false";

			if (FindUserProfile.Find(userName) == null)
				return "mustregister";
			
			var nResp = UpdateSiteNewsletterOptIn.Update(userName, true);

			return nResp? "true" : "false";
		}

		[HttpGet]
		public bool IsUserSignedUp()
		{
			return NewsletterOptedInContext.OptedIn;
		}
	}
}