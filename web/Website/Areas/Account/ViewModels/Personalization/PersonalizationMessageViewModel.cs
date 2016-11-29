﻿namespace Informa.Web.Areas.Account.ViewModels.Personalization
{
    using System;
    using System.Linq;
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
        private const string DISMISS_COOKIE_NAME = "dismiss_cookie";
        public PersonalizationMessageViewModel(ITextTranslator textTranslator, IUserCompanyContext userCompanyContext,
            ISiteRootContext siterootContext, IAuthenticatedUserContext authenticatedUserContext, IUserPreferenceContext userPreferences)
        {
            TextTranslator = textTranslator;
            UserCompanyContext = userCompanyContext;
            SiterootContext = siterootContext;
            AuthenticatedUserContext = authenticatedUserContext;
            UserPreferences = userPreferences;
        }
        public string RightRailMessage => GetMessageForRightRail();
        public string LandingPageMessage => GetMessageForLandingPage();
        public string ArticlePageMessage => GetMessageForArticlePage();
        public string MyViewSettingsPageUrl => SiterootContext.Item?.MyView_Settings_Page?._Url;
        public string MyViewSettingsText => TextTranslator.Translate("Personalization.MyViewSettingsText");
        public string WelcomeMessageLinkText => TextTranslator.Translate("Personalization.WelcomeMessageLinkText");
        public string MyViewText => TextTranslator.Translate("Personalization.MyViewLinkText");
        public string DismissText => TextTranslator.Translate("Personalization.DismissText");
        public string PageName => SiterootContext.Item._Name;
        public string FindOutMoreText => TextTranslator.Translate("Personalization.FindOutMoreText");
        public string FindOutMoreUrl => SiterootContext.Item?.Welcome_Message_FindOutMore_LinkUrl?._Url;
        public bool IsGlobalToggleEnabled => SiterootContext.Item.Enable_MyView_Toggle;
        public string MyViewLinkUrl => SiterootContext.Item?.Welcome_Message_MyView_LinkUrl?._Url;
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public bool HideWelcomeMessage => GetWelcomeMessageDisplayStatus();

        /// <summary>
        /// Gets the display message status i.e whether to display personalization message or not.
        /// </summary>
        /// <returns>Personalization message display status</returns>
        private bool GetWelcomeMessageDisplayStatus()
        {
            if(IsAuthenticated)
            {
                return ((UserPreferences.Preferences != null &&
                       UserPreferences.Preferences.PreferredChannels != null &&
                       UserPreferences.Preferences.PreferredChannels.Count > 0) &&
                       (!UserPreferences.Preferences.IsNewUser || 
                       (UserPreferences.Preferences.IsNewUser && 
                       UserPreferences.Preferences.PreferredChannels.Any(channel => channel.IsFollowing))) &&
                       Convert.ToInt32(SiterootContext.Item.Welcome_Message_Display_Frequency) > (DateTime.Now - Convert.ToDateTime(UserPreferences.Preferences.LastUpdateOn)).TotalDays) ||
                       System.Web.HttpContext.Current.Request.Cookies[DISMISS_COOKIE_NAME] != null;
            }
            return true;
        }

        /// <summary>
        /// Gets personalization message to right rail component
        /// </summary>
        /// <returns></returns>
        private string GetMessageForRightRail()
        {
            if (UserCompanyContext?.Company?.Type == CompanyType.TransparentIP)
                return TextTranslator.Translate("IPAuthenticatedUser.RightRailMessage");

            else if (UserCompanyContext?.Company?.Type == null)
                return TextTranslator.Translate("RegisteredUser.RightRailMessage");

            return TextTranslator.Translate("CorporateUser.RightRailMessage");
        }

        /// <summary>
        /// Gets personalization message for landing page component
        /// </summary>
        /// <returns></returns>
        private string GetMessageForLandingPage()
        {
            if (UserCompanyContext?.Company?.Type == CompanyType.TransparentIP)
                return TextTranslator.Translate("IPAuthenticatedUser.LandingPageMessage");

            else if (UserCompanyContext?.Company?.Type == null)
                return TextTranslator.Translate("RegisteredUser.LandingPageMessage");

            return TextTranslator.Translate("CorporateUser.LandingPageMessage");
        }

        /// <summary>
        /// Gets personalization message for article page component
        /// </summary>
        /// <returns></returns>
        private string GetMessageForArticlePage()
        {
            if (UserCompanyContext?.Company?.Type == CompanyType.TransparentIP)
                return TextTranslator.Translate("IPAuthenticatedUser.ArticlePageMessage");

            else if (UserCompanyContext?.Company?.Type == null)
                return TextTranslator.Translate("RegisteredUser.ArticlePageMessage");

            return TextTranslator.Translate("CorporateUser.ArticlePageMessage");
        }
    }
}