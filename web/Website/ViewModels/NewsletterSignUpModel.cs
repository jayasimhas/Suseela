using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Newsletter;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;


namespace Informa.Web.ViewModels
{
	public class NewsletterSignUpModel : GlassViewModel<INewsletter_Sign_Up>
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IGlobalSitecoreService GlobalService;
		protected readonly ISiteNewsletterUserOptedInContext NewsletterOptedInContext;
		protected readonly IReturnUrlContext ReturnURLContext;

		public NewsletterSignUpModel(
			IAuthenticatedUserContext userContext,
			ITextTranslator textTranslator,
			ISiteRootContext siteRootContext,
			IGlobalSitecoreService globalService,
            ISiteNewsletterUserOptedInContext newsletterOptedInContext,
			IReturnUrlContext returnURLContext)

		{

			UserContext = userContext;
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			GlobalService = globalService;
			NewsletterOptedInContext = newsletterOptedInContext;
            ReturnURLContext = returnURLContext;
        }

		public bool IsAuthenticated => UserContext.IsAuthenticated;

		public bool HasSubscribed => NewsletterOptedInContext.OptedIn;
		public string PreferencesURL => GlobalService.GetItem<I___BasePage>(SiteRootContext.Item.Email_Preferences_Page)?._Url ?? "#";
		public string GeneralError => TextTranslator.Translate("Newsletter.GeneralError");
		public string NewsletterSignupEmail => TextTranslator.Translate("Global.NewsletterSignupEmail");
		public string ManagePreferences => TextTranslator.Translate("Global.ManagePreferences");


		public string InvalidEmailFormat => TextTranslator.Translate("Newsletter.InvalidEmailFormat");
		public string NeedsRegistrationMessage => TextTranslator.Translate("Newsletter.NeedsRegistrationMessage");
		public string NeedsRegistrationButtonText => TextTranslator.Translate("Newsletter.NeedsRegistrationButtonText");
		public string RegistrationURL => !string.IsNullOrEmpty(SiteRootContext.Item.Register_Link?.Url) ? SiteRootContext.Item.Register_Link?.Url + $"?returnURL={(string.IsNullOrEmpty(ReturnURLContext.Url) ? "/" : ReturnURLContext.Url)}&username=" : "#";

	}
}