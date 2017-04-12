﻿using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Offer;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Collections.Generic;
using Informa.Library.ViewModels.Account;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class EmailPreferencesViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IOfferUserOptedInContext OffersOptedInContext;
        protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IPublicationsNewsletterUserOptInContext PublicationNewsletterUserOptInContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public EmailPreferencesViewModel(
            ITextTranslator translator,
			IOfferUserOptedInContext offersOptedInContext,
            ISignInViewModel signInViewModel,
            IAuthenticatedUserContext userContext,
			IPublicationsNewsletterUserOptInContext publicationNewsletterUserOptInContext,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            TextTranslator = translator;
            OffersOptedInContext = offersOptedInContext;
            UserContext = userContext;
            SignInViewModel = signInViewModel;
			PublicationNewsletterUserOptInContext = publicationNewsletterUserOptInContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

		public readonly ISignInViewModel SignInViewModel;
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public bool DoNotSendOfferEmails => !OffersOptedInContext.OptedIn.OptIn;
		public string NewsletterTitle => TextTranslator.Translate("Preferences.NewsletterTitle");
		public string NewsletterOptInTitleHeading => TextTranslator.Translate("Preferences.NewsletterOptInTitleHeading");
		public string NewsletterOptInReceiveEmailHeading => TextTranslator.Translate("Preferences.NewsletterOptInReceiveEmailHeading");
        public string GeneralErrorText => TextTranslator.Translate("Preferences.GeneralError");
        public string EmailsLabel => TextTranslator.Translate("Preferences.EmailsLabel");
        public string SubmitText => TextTranslator.Translate("Preferences.SubmitText");
        public string PreferencesSavedMessage => TextTranslator.Translate("Preferences.PreferencesSavedMessage");
        public string DailyEmailLabel => TextTranslator.Translate("Preferences.DailyEmailLabel");
        public string WeeklyEmailLabel => TextTranslator.Translate("Preferences.WeeklyEmailLabel");
        public bool IsNewSalesforceEnabled => SalesforceConfigurationContext.IsNewSalesforceEnabled;
        public IEnumerable<IPublicationNewsletterUserOptIn> PublicationNewsletterOptIns => PublicationNewsletterUserOptInContext.OptIns;
    }
}