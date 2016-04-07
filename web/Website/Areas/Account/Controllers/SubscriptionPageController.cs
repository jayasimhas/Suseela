using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.Areas.Account.ViewModels.Subscription;
using Sitecore.Data.Items;
using System;
using System.Web.Mvc;
using Informa.Library.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.User.Newsletter;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Informa.Web.ViewModels;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SubscriptionPageController : Controller
    {
		// GET: Account/SubscriptionPage
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly IItemReferences _itemReferences;
        private readonly ISitecoreContext SitecoreContext;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly ISignInViewModel SignInViewModel;
        protected readonly IUpdateOfferUserOptIn OffersOptIn;
		protected readonly ISiteNewsletterTypeContext NewsletterTypeContext;
		protected readonly IUpdateSiteNewsletterUserOptInContext UpdateNewsletterOptInContext;
		protected readonly IUpdateSiteNewsletterUserOptIn UpdateNewsletterOptIn;

		public SubscriptionPageController(
            Func<string, ISitecoreService> sitecoreFactory, 
            IItemReferences itemReferences,
            ISitecoreContext sitecoreContext,
            IAuthenticatedUserContext userContext,
            ISignInViewModel signInViewModel,
            IUpdateOfferUserOptIn offersOptIn,
			ISiteNewsletterTypeContext newsletterTypeContext,
			IUpdateSiteNewsletterUserOptInContext updateNewsletterOptInContext,
			IUpdateSiteNewsletterUserOptIn updateNewsletterOptIn)
		{
            _sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_itemReferences = itemReferences;
		    SitecoreContext = sitecoreContext;
            UserContext = userContext;
            SignInViewModel = signInViewModel;
            OffersOptIn = offersOptIn;
			NewsletterTypeContext = newsletterTypeContext;
			UpdateNewsletterOptInContext = updateNewsletterOptInContext;
			UpdateNewsletterOptIn = updateNewsletterOptIn;
		}
		public ActionResult Index(string pub)
        {
			//TODO: Add logic to subscribe the user using salesforce and also check for logged in user functionality
			if (!string.IsNullOrEmpty(pub) && pub.ToLower() == NewsletterTypeContext.Type.ToString().ToLower())
			{
				SubscriptionModel subscriptionModel = new SubscriptionModel();
				var item = _sitecoreMasterService.GetItem<I___BasePage>(_itemReferences.SubscriptionPage);
				subscriptionModel.BodyText = item.Body;
				return View("~/Areas/Account/Views/Management/Subscriptions.cshtml", subscriptionModel);
			}
			var emailPreferncesItem = _sitecoreMasterService.GetItem<Item>(_itemReferences.EmailPreferences);
            return Redirect("/accounts/email-preferences"); // TODO Remove harcoded path
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
			var newsletterType = NewsletterTypeContext.Type;

            if (!string.IsNullOrEmpty(Pub) 
                && (Pub.ToLower() == newsletterType.ToDescriptionString().ToLower() || Pub.ToLower() == newsletterType.ToString().ToLower()))
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
			var newsletterType = NewsletterTypeContext.Type;

			if (Type.ToLower() == "newsletter" && !string.IsNullOrEmpty(Pub) && (Pub.ToLower() == newsletterType.ToDescriptionString().ToLower() || Pub.ToLower() == newsletterType.ToString().ToLower())) {
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
					User = UserContext.User.Username;
				}

				if (!string.IsNullOrWhiteSpace(User))
				{
					OffersOptIn.Update(User, false);
				}
            }

            return View(OneClickView, s);
        }
    }
}