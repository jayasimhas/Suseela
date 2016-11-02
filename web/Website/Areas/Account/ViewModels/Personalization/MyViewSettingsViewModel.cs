﻿namespace Informa.Web.Areas.Account.ViewModels.Personalization
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

        public string SectionDescription => GlassModel?.Body;
    }
}
