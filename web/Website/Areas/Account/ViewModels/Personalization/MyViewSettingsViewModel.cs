namespace Informa.Web.Areas.Account.ViewModels.Personalization
{

    using Library.Globalization;
    using Library.User.Authentication;
    using Library.ViewModels.Account;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Library.Site;
    using Web.ViewModels;

    public class MyViewSettingsViewModel : GlassViewModel<IMy_View_Settings_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public readonly ICallToActionViewModel CallToActionViewModel;
        public readonly IChannelsViewModel ChannelsViewModel;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISiteRootContext SiteRootContext;

        public MyViewSettingsViewModel(
                ITextTranslator translator,
                ICallToActionViewModel callToActionViewModel,
                IAuthenticatedUserContext userContext,
                IChannelsViewModel channelsViewModel,
                ISiteRootContext siteRootContext)
        {
            TextTranslator = translator;
            CallToActionViewModel = callToActionViewModel;
            AuthenticatedUserContext = userContext;
            ChannelsViewModel = channelsViewModel;
            SiteRootContext = siteRootContext;
        }

        /// <summary>
        /// Gets a value indicating whether user is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if user is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => GlassModel?.Title;

        /// <summary>
        /// Gets the save button text.
        /// </summary>
        /// <value>
        /// The save button text.
        /// </value>
        public string SaveButtonText => TextTranslator.Translate("MyViewSettings.SaveButtonText");

        /// <summary>
        /// Gets the go to my view button text.
        /// </summary>
        /// <value>
        /// The go to my view button text.
        /// </value>
        public string GoToMyViewButtonText => TextTranslator.Translate("MyViewSettings.GoToMyViewButtonText");

        /// <summary>
        /// Gets the save preferences message.
        /// </summary>
        /// <value>
        /// The save preferences message.
        /// </value>
        public string SavePreferencesMessage => TextTranslator.Translate("MyViewSettings.SavePreferencesMessage");

        /// <summary>
        /// Gets the select preferences message.
        /// </summary>
        /// <value>
        /// The select preferences message.
        /// </value>
        public string SelectPreferencesMessage => TextTranslator.Translate("MyViewSettings.SelectPreferencesMessage");

        /// <summary>
        /// Gets the section description.
        /// </summary>
        /// <value>
        /// The section description.
        /// </value>
        public string SectionDescription => GlassModel?.Body;

        /// <summary>
        /// Gets a value indicating whether this instance is from registration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is from registration; otherwise, <c>false</c>.
        /// </value>
        public bool IsFromRegistration => GlassModel?.IsFromRegistration ?? false;

        /// <summary>
        /// Gets the help link detail.
        /// </summary>
        /// <value>
        /// The help link detail.
        /// </value>
        public string HelpLinkDetail => GlassModel?.Help_Link;
        /// <summary>
        /// Gets the complete your registration text.
        /// </summary>
        /// <value>
        /// The complete your registration text.
        /// </value>
        public string CompleteYourRegistrationText => TextTranslator.Translate("Registration.OptIn.CompleteYourRegistration");
        /// <summary>
        /// Gets the preference not saved error.
        /// </summary>
        /// <value>
        /// The preference not saved error.
        /// </value>
        public string PreferenceNotSavedError => TextTranslator.Translate("Registration.Optin.PreferenceNotSaved");
        /// <summary>
        /// Gets the not followed error message.
        /// </summary>
        /// <value>
        /// The not followed error message.
        /// </value>
        public string NotFollowedError => GlassModel?.Not_Followed_Error;
        /// <summary>
        /// Gets a value indicating whether [enable save preferences check].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable save preferences check]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableSavePreferencesCheck => GlassModel?.EnableSavePreferencesCheck ?? false;
        /// <summary>
        /// Gets my view page URL.
        /// </summary>
        /// <value>
        /// My view page URL.
        /// </value>
        public string MyViewPageUrl => SiteRootContext?.Item?.MyView_Page?._Url;

        /// <summary>
        /// Gets the popup cancel button text.
        /// </summary>
        /// <value>
        /// The popup cancel button text.
        /// </value>
        public string PopupCancelButtonText => TextTranslator.Translate("MyViewSettings.SaveSettingsAlertPopupCancelButtonText");

        /// <summary>
        /// Gets the popup yes button text.
        /// </summary>
        /// <value>
        /// The popup yes button text.
        /// </value>
        public string PopupYesButtonText => TextTranslator.Translate("MyViewSettings.SaveSettingsAlertPopupYesButtonText");

        /// <summary>
        /// Gets the popup no button text.
        /// </summary>
        /// <value>
        /// The popup no button text.
        /// </value>
        public string PopupNoButtonText => TextTranslator.Translate("MyViewSettings.SaveSettingsAlertPopupNoButtonText");

        /// <summary>
        /// Gets the save settings alert header text.
        /// </summary>
        /// <value>
        /// The save settings alert header text.
        /// </value>
        public string SaveSettingsAlertHeaderText => TextTranslator.Translate("MyViewSettings.SaveSettingsAlertPopupHeaderText");

        /// <summary>
        /// Gets the save settings alert message text.
        /// </summary>
        /// <value>
        /// The save settings alert message text.
        /// </value>
        public string SaveSettingsAlertMessageText => TextTranslator.Translate("MyViewSettings.SaveSettingsAlertPopupMessage");
    }
}

