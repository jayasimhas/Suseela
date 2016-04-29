using System.Web.Http;
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
		protected readonly INewsletterUserOptInsContext NewsletterUserOptInsContext;
		protected readonly IUpdateNewsletterUserOptInsContext UpdateNewsletterOptInsContext;

		public PreferencesApiController(
			IUpdateOfferUserOptInContext offersOptIn,
			IUpdateSiteNewsletterUserOptIn updateSiteNewsletterOptIn,
			ISiteNewsletterUserOptedInContext newsletterOptedInContext,
			INewsletterUserOptInsContext newsletterUserOptInsContext,
			IUpdateNewsletterUserOptInsContext updateNewsletterOptInsContext)
		{
			OffersOptIn = offersOptIn;
			UpdateSiteNewsletterOptIn = updateSiteNewsletterOptIn;
			NewsletterOptedInContext = newsletterOptedInContext;
			NewsletterUserOptInsContext = newsletterUserOptInsContext;
			UpdateNewsletterOptInsContext = updateNewsletterOptInsContext;
		}

		[HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult Update(PreferencesRequest request)
		{
			var newsletterUpdated = true;

			if (request.NewsletterOptIns != null)
			{
				var newsletterUserOptins = NewsletterUserOptInsContext.OptIns.ToList();

				newsletterUserOptins.ForEach(noi => noi.OptIn = request.NewsletterOptIns.Contains(noi.NewsletterType));

				newsletterUpdated = UpdateNewsletterOptInsContext.Update(newsletterUserOptins);
			}

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