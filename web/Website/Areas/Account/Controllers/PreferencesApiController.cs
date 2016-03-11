using System.Collections.Generic;
using System.Web.Http;
using Informa.Library.Newsletter;
using Informa.Library.Site.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Web.Areas.Account.Models.User.Management;


namespace Informa.Web.Areas.Account.Controllers
{
	public class PreferencesApiController : ApiController
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUpdateNewsletterUserOptIn NewsletterOptIn;
		protected readonly INewsletterUserOptInFactory NewsletterUserOptInFactory;
		protected readonly ISiteNewsletterTypesContext NewsletterTypesContext;
		protected readonly IUpdateOfferUserOptIn OffersOptIn;

		public PreferencesApiController(
			IAuthenticatedUserContext userContext,
			IUpdateNewsletterUserOptIn newsletterOptIn,
			INewsletterUserOptInFactory newsletterUserOptInFactory,
			ISiteNewsletterTypesContext newsletterTypesContext,
			IUpdateOfferUserOptIn offersOptIn)
		{
			UserContext = userContext;
			NewsletterOptIn = newsletterOptIn;
			NewsletterUserOptInFactory = newsletterUserOptInFactory;
			NewsletterTypesContext = newsletterTypesContext;
			OffersOptIn = offersOptIn;
		}

		[HttpPost]
		public IHttpActionResult Update(PreferencesRequest request)
		{
			var userNewsletterOptIns = new List<INewsletterUserOptIn>() { NewsletterUserOptInFactory.Create(NewsletterType.Scrip, request.NewsletterOptIn) };
			var nResp = NewsletterOptIn.Update(userNewsletterOptIns, request.UserName);
			var oResp = OffersOptIn.Update(UserContext.User, !request.DoNotSendOffersOptIn);

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