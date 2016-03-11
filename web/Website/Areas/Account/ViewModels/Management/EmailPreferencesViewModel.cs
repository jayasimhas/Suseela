using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
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
        public readonly IQueryNewsletterUserOptIn NewsletterOptIn;
        public readonly IQueryOfferUserOptIn OffersOptIn;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly ISignInViewModel SignInViewModel;
        
        public EmailPreferencesViewModel(
            ITextTranslator translator,
            IQueryNewsletterUserOptIn newsletterOptIn,
            IQueryOfferUserOptIn offersOptIn,
            ISignInViewModel signInViewModel,
            IAuthenticatedUserContext userContext)
        {
            TextTranslator = translator;
            NewsletterOptIn = newsletterOptIn;
            OffersOptIn = offersOptIn;
            UserContext = userContext;
            SignInViewModel = signInViewModel;

            //check if any of the query results is the one we're looking for and if so use that value else default to false
            var userNewsOptInStatus = NewsletterOptIn.Query(UserContext.User);
            if (userNewsOptInStatus.Success)
            {
                var result = userNewsOptInStatus.NewsletterOptIns.Where(a => a.Name.ToLower().Equals(NewsletterType.Scrip.ToString().ToLower()));
                ReceivesNewsletterEmails = (result.Any())
                    ? result.First().ReceivesNewsletterAlert
                    : false;
            }
            else
            {
                ReceivesNewsletterEmails = false;
            }
            
            var userOffersOptInStatus = OffersOptIn.Query(UserContext.User);
            DoNotSendOfferEmails = (userOffersOptInStatus.Success) && userOffersOptInStatus.DoNotSendOffers;
        }

        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public bool ReceivesNewsletterEmails { get; set; }
        public bool DoNotSendOfferEmails { get; set; }
        public string Title => GlassModel?.Title;
        public string GeneralErrorText => TextTranslator.Translate("Preferences.GeneralError");
        public string NewsletterLabel => TextTranslator.Translate("Preferences.NewsletterLabel");
        public string EmailsLabel => TextTranslator.Translate("Preferences.EmailsLabel");
        public string SubmitText => TextTranslator.Translate("Preferences.SubmitText");
        public string PreferencesSavedMessage => TextTranslator.Translate("Preferences.PreferencesSavedMessage");
    }
}
