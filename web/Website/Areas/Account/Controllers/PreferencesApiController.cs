using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Informa.Library.Site.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.User.Registration;
using Informa.Library.User.Registration.Web;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Web.Areas.Account.Models.User.Management;
using Informa.Web.Areas.Account.Models.User.Registration;

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
            IUpdateOfferUserOptIn offersOptIn)
        {
            UserContext = userContext;
            NewsletterOptIn = newsletterOptIn;
            OffersOptIn = offersOptIn;
        }

        [HttpPost]
        public IHttpActionResult Update(PreferencesRequest request)
        {
            var userNewsletterOptIns = NewsletterTypesContext.NewsletterTypes.Select(nt => NewsletterUserOptInFactory.Create(nt, request.NewsletterOptIn));
            var nResp = NewsletterOptIn.Update(UserContext.User, userNewsletterOptIns);
            var oResp = OffersOptIn.Update(UserContext.User, !request.DoNotSendOffersOptIn);
            
            var success = nResp && oResp;

            return Ok(new
            {
                success = success
            });
        }
    }
}