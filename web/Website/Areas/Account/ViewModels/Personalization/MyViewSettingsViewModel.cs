namespace Informa.Web.Areas.Account.ViewModels.Personalization
{

    using Library.Globalization;
    using Library.User.Authentication;
    using Library.ViewModels.Account;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
    using Jabberwocky.Glass.Autofac.Mvc.Models;

    public class MyViewSettingsViewModel : GlassViewModel<IMy_View_Settings_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public readonly ISignInViewModel SignInViewModel;
        public readonly IChannelsViewModel ChannelsViewModel;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

        public MyViewSettingsViewModel(
                ITextTranslator translator,
                ISignInViewModel signInViewModel,
                IAuthenticatedUserContext userContext,
                IChannelsViewModel channelsViewModel)
        {
            TextTranslator = translator;
            SignInViewModel = signInViewModel;
            AuthenticatedUserContext = userContext;
            ChannelsViewModel = channelsViewModel;
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
        public string NotFollowedError => GlassModel?.Not_Followed_Error;

    }
}

