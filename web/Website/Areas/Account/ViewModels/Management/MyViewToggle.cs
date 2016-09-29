namespace Informa.Web.Areas.Account.ViewModels.Management
{
    using System;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Jabberwocky.Glass.Models;
    using Library.Globalization;
    using Library.Site;
    using Library.User.Authentication;
    using Web.ViewModels;
    using Library.User.UserPreference;
    public class MyViewToggle : GlassViewModel<IGlassBase>
    {
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IUserPreferenceContext UserPreferences;
        public MyViewToggle(ISiteRootContext siterootContext, IAuthenticatedUserContext authenticatedUserContext,
                            ICallToActionViewModel callToActionViewModel, ITextTranslator textTranslator, IUserPreferenceContext userPreferences)
        {
            SiterootContext = siterootContext;
            AuthenticatedUserContext = authenticatedUserContext;
            CallToActionViewModel = callToActionViewModel;
            TextTranslator = textTranslator;
            UserPreferences = userPreferences;
        }

        public string MyViewLinkText => TextTranslator.Translate("MyView.MyViewLink");

        public bool IsGlobalToggleEnabled => SiterootContext.Item.Enable_MyView_Toggle;
        public string MyViewLinkURL => GetNavigationUrl();
        private string GetNavigationUrl()
        {
            if(IsAuthenticated)
            {
                if (UserPreferences.Preferences != null &&
                    UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count > 0)
                {
                    //Take user to Personalized home page.
                    return "/personal-home";
                }
                else
                {
                    //Take to MyView settings page
                    return SiterootContext.Item?.MyView_Settings_Page?._Url;
                }
            }
            else
            {
                return "/";
            }
        }
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
    }
}