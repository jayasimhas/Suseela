using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class EmailPreferencesViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IOfferUserOptedInContext OffersOptedInContext;
        protected readonly IAuthenticatedUserContext UserContext;
		protected readonly ISiteNewsletterUserOptedInContext NewsletterOptedInContext;

		public EmailPreferencesViewModel(
            ITextTranslator translator,
			IOfferUserOptedInContext offersOptedInContext,
            ISignInViewModel signInViewModel,
            IAuthenticatedUserContext userContext,
			ISiteNewsletterUserOptedInContext newsletterOptedInContext)
        {
            TextTranslator = translator;
            OffersOptedInContext = offersOptedInContext;
            UserContext = userContext;
            SignInViewModel = signInViewModel;
			NewsletterOptedInContext = newsletterOptedInContext;
        }

		public readonly ISignInViewModel SignInViewModel;
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public bool ReceivesNewsletterEmails => NewsletterOptedInContext.OptedIn;
		public bool DoNotSendOfferEmails => !OffersOptedInContext.OptedIn;
        public string Title => GlassModel?.Title;
        public string GeneralErrorText => TextTranslator.Translate("Preferences.GeneralError");
        public string NewsletterLabel => TextTranslator.Translate("Preferences.NewsletterLabel");
        public string EmailsLabel => TextTranslator.Translate("Preferences.EmailsLabel");
        public string SubmitText => TextTranslator.Translate("Preferences.SubmitText");
        public string PreferencesSavedMessage => TextTranslator.Translate("Preferences.PreferencesSavedMessage");
    }
}
