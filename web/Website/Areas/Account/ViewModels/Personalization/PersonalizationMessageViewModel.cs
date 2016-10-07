namespace Informa.Web.Areas.Account.ViewModels.Personalization
{
    using System;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Jabberwocky.Glass.Models;
    using Library.Company;
    using Library.Globalization;
    using Library.Site;
    using Library.User.Authentication;
    using Library.User.UserPreference;
    public class PersonalizationMessageViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserPreferenceContext UserPreferences;
        public PersonalizationMessageViewModel(ITextTranslator textTranslator, IUserCompanyContext userCompanyContext,
            ISiteRootContext siterootContext, IAuthenticatedUserContext authenticatedUserContext, IUserPreferenceContext userPreferences)
        {
            TextTranslator = textTranslator;
            UserCompanyContext = userCompanyContext;
            SiterootContext = siterootContext;
            AuthenticatedUserContext = authenticatedUserContext;
            UserPreferences = userPreferences;
        }
        public string MessageSmall => GetMessageForRightRail();
        public string MessageFull => GetMessageForFullView();
        public string MyViewText => TextTranslator.Translate("MyView.MyViewLink");
        public string DismissText => TextTranslator.Translate("Personalization.DismissText");
        public string FindOutMoreText => TextTranslator.Translate("Personalization.FindOutMoreText");
        public bool IsGlobalToggleEnabled => SiterootContext.Item.Enable_MyView_Toggle;
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public bool PreferencesExists => UserHasPreferences();

        private bool UserHasPreferences()
        {
            if(IsAuthenticated)
            {
                return UserPreferences.Preferences != null &&
                       UserPreferences.Preferences.PreferredChannels != null &&
                       UserPreferences.Preferences.PreferredChannels.Count > 0;
            }
            return false;
        }

        private string GetMessageForRightRail()
        {
            if (UserCompanyContext?.Company?.Type == CompanyType.TransparentIP)
                return TextTranslator.Translate("IPAuthenticatedUser.RightRailMessage");

            else if (UserCompanyContext?.Company?.Type == null)
                return TextTranslator.Translate("FreeTrialUser.RightRailMessage");

            return TextTranslator.Translate("CorporateUser.RightRailMessage");
        }

        private string GetMessageForFullView()
        {
            if (UserCompanyContext?.Company?.Type == CompanyType.TransparentIP)
                return TextTranslator.Translate("IPAuthenticatedUser.FullViewMessage");

            else if (UserCompanyContext?.Company?.Type == null)
                return TextTranslator.Translate("FreeTrialUser.FullViewMessage");

            return TextTranslator.Translate("CorporateUser.FullViewMessage");
        }
    }
}