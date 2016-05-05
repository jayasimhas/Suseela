using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.Areas.Account.ViewModels.Subscription;
using Sitecore.Data.Items;
using System;
using System.Web.Mvc;
using Informa.Library.Publication;
using Informa.Library.User.Authentication;
using Informa.Library.User.Offer;
using Informa.Library.User.Newsletter;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Informa.Web.ViewModels;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SubscriptionPageController : Controller
    {
		// GET: Account/SubscriptionPage
		private readonly ISitecoreContext SitecoreContext;

        protected readonly IUpdateOfferUserOptIn OffersOptIn;
		protected readonly IUpdateOfferUserOptInContext OffersOptInContext;
		protected readonly ISitePublicationContext NewsletterTypeContext;
		protected readonly IUpdateSiteNewsletterUserOptInContext UpdateNewsletterOptInContext;
		protected readonly IUpdateSiteNewsletterUserOptIn UpdateNewsletterOptIn;

		public readonly IAuthenticatedUserContext UserContext;
		public readonly ISignInViewModel SignInViewModel;


        public SubscriptionPageController(
            ISitecoreContext sitecoreContext,
            IAuthenticatedUserContext userContext,
            ISignInViewModel signInViewModel,
            IUpdateOfferUserOptIn offersOptIn,
			IUpdateOfferUserOptInContext offersOptInContext,
			ISitePublicationContext newsletterTypeContext,
			IUpdateSiteNewsletterUserOptInContext updateNewsletterOptInContext,
			IUpdateSiteNewsletterUserOptIn updateNewsletterOptIn)
		{
			SitecoreContext = sitecoreContext;
            UserContext = userContext;
            SignInViewModel = signInViewModel;
            OffersOptIn = offersOptIn;
			OffersOptInContext = offersOptInContext;
			NewsletterTypeContext = newsletterTypeContext;
			UpdateNewsletterOptInContext = updateNewsletterOptInContext;
			UpdateNewsletterOptIn = updateNewsletterOptIn;
		}
        
		
        public ActionResult Subscribe(string Pub)
        {
            string OneClickView = "~/Areas/Account/Views/Management/OneClickSubscribe.cshtml";
            var page = SitecoreContext.GetCurrentItem<ISubscribe_Page>();
            
            SubscriptionModel s = new SubscriptionModel();
            s.BodyText = page.Body.Replace("#USER_EMAIL#", UserContext?.User?.Username ?? string.Empty);
            s.IsAuthenticated = UserContext.IsAuthenticated;
            s.SignInViewModel = SignInViewModel;

            if (!UserContext.IsAuthenticated)
            {
                //redirect to email preferences
                string url = page?._Parent?._Url ?? string.Empty;
                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
                else
                    return View(OneClickView, s);
            }

			//process subscribe
			if (!string.IsNullOrEmpty(Pub) 
                && (Pub.ToLower() == NewsletterTypeContext.Name.ToLower()))
            {
				UpdateNewsletterOptInContext.Update(true);
            }
            
            return View(OneClickView, s);
        }

        public ActionResult Unsubscribe(string User, string Type, string Pub)
        {
            string OneClickView = "~/Areas/Account/Views/Management/OneClickUnsubscribe.cshtml";
            var page = SitecoreContext.GetCurrentItem<IUnsubscribe_Page>();

            SubscriptionModel s = new SubscriptionModel();
            s.BodyText = page.Body;
            s.SignInViewModel = SignInViewModel;

			if (string.IsNullOrEmpty(Type))
			{
				return View(OneClickView, s);
			}

			//process unsubscribe
			var newsletterType = NewsletterTypeContext.Name;

			if (Type.ToLower() == "newsletter" && !string.IsNullOrEmpty(Pub) && (Pub.ToLower() == newsletterType.ToLower())) {
				if (UserContext.IsAuthenticated)
				{
					UpdateNewsletterOptInContext.Update(false);
				}
				else if (!string.IsNullOrWhiteSpace(User))
				{
					UpdateNewsletterOptIn.Update(User, false);
				}
            }
            else if (Type.ToLower() == "promotions") {
				if (UserContext.IsAuthenticated)
				{
					OffersOptInContext.Update(false);
				}
				else if (!string.IsNullOrWhiteSpace(User))
				{
					OffersOptIn.Update(User, false);
				}
            }

            return View(OneClickView, s);
        }
    }
}