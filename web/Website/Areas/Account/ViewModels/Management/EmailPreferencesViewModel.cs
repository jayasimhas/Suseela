using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Common;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class EmailPreferencesViewModel : GlassViewModel<I___BasePage>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly ISitecoreContext SitecoreContext;
        public readonly IQueryNewsletterUserOptIn NewsletterOptIn;
        public readonly IQueryOfferUserOptIn OffersOptIn;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly ISignInViewModel SignInViewModel;

        public EmailPreferencesViewModel(
            ITextTranslator translator, 
            ISitecoreContext sitecoreContext,
            IQueryNewsletterUserOptIn newsletterOptIn,
            IQueryOfferUserOptIn offersOptIn,
            IAuthenticatedUserContext userContext,
            ISignInViewModel signInViewModel)
        {
            TextTranslator = translator;
            SitecoreContext = sitecoreContext;
            NewsletterOptIn = newsletterOptIn;
            OffersOptIn = offersOptIn;
            UserContext = userContext;
            SignInViewModel = signInViewModel;

            var userNewsOptInStatus = NewsletterOptIn.Query(UserContext.User);
            if (userNewsOptInStatus.Success)
                ReceivesNewsletterEmails = userNewsOptInStatus.NewsletterOptIns.Any() && userNewsOptInStatus.NewsletterOptIns.First().ReceivesNewsletterAlert;

            var userOffersOptInStatus = OffersOptIn.Query(UserContext.User);
            if (userOffersOptInStatus.Success)
                DoNotSendOfferEmails = userOffersOptInStatus.DoNotSendOffers;
        }

        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public bool ReceivesNewsletterEmails { get; set; }
        public bool DoNotSendOfferEmails { get; set; }
        public string GeneralErrorText => TextTranslator.Translate("Preferences.GeneralError");
        public string NewsletterLabel => TextTranslator.Translate("Preferences.NewsletterLabel");
        public string EmailsLabel => TextTranslator.Translate("Preferences.EmailsLabel");
        public string SubmitText => TextTranslator.Translate("Preferences.SubmitText");
        public string PreferencesSavedMessage => TextTranslator.Translate("Preferences.PreferencesSavedMessage");
    }
}