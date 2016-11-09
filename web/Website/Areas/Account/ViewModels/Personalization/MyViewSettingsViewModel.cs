namespace Informa.Web.Areas.Account.ViewModels.Personalization
{

    using Library.Globalization;
    using Library.User.Authentication;
    using Library.ViewModels.Account;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Library.Site;

    public class MyViewSettingsViewModel : GlassViewModel<IMy_View_Settings_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public readonly ISignInViewModel SignInViewModel;
        public readonly IChannelsViewModel ChannelsViewModel;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISiteRootContext SiteRootContext;

        public MyViewSettingsViewModel(
                ITextTranslator translator,
                ISignInViewModel signInViewModel,
                IAuthenticatedUserContext userContext,
                IChannelsViewModel channelsViewModel,
                ISiteRootContext siteRootContext)
        {
            TextTranslator = translator;
            SignInViewModel = signInViewModel;
            AuthenticatedUserContext = userContext;
            ChannelsViewModel = channelsViewModel;
            SiteRootContext = siteRootContext;
        }

        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public string Title => GlassModel?.Title;

        public string SaveButtonText => TextTranslator.Translate("MyViewSettings.SaveButtonText");

        public string GoToMyViewButtonText => TextTranslator.Translate("MyViewSettings.GoToMyViewButtonText");

        public string SavePreferencesMessage => TextTranslator.Translate("MyViewSettings.SavePreferencesMessage");

        public string SelectPreferencesMessage => TextTranslator.Translate("MyViewSettings.SelectPreferencesMessage");

        public string SectionDescription => GlassModel?.Body;

        public bool IsFromRegistration => GlassModel?.IsFromRegistration ?? false;
        public string HelpLinkDetail => GlassModel?.Help_Link;
        public string CompleteYourRegistrationText => TextTranslator.Translate("Registration.OptIn.CompleteYourRegistration");
        public string PreferenceNotSavedError => TextTranslator.Translate("Registration.Optin.PreferenceNotSaved");
        public string NotFollowedError => GlassModel?.Not_Followed_Error;
        public bool EnableSavePreferencesCheck => GlassModel?.EnableSavePreferencesCheck ?? false;
        public string MyViewPageUrl => SiteRootContext?.Item?.MyView_Page?._Url;
        public string PopupSaveButtonText => TextTranslator.Translate("MyViewSettings.Popup.Save.Button.Text");
        public string PopupCancelButtonText => TextTranslator.Translate("MyViewSettings.Popup.Cancel.Button.Text");
    }
}

