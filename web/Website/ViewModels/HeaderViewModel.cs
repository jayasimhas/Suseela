﻿using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Library.Company;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global;
using System.Web;
using Informa.Library.Services.Global;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.SalesforceConfiguration;
using System;
using Informa.Library.ViewModels.Account;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;


namespace Informa.Web.ViewModels
{
    public class HeaderViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserCompanyNameContext CompanyNameContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IUserProfileContext ProfileContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public HeaderViewModel(
            IAuthenticatedUserContext authenticatedUserContext,
            IUserCompanyNameContext companyNameContext,
            ITextTranslator textTranslator,
            ISiteRootContext siteRootContext,
            IUserProfileContext profileContext,
            IGlobalSitecoreService globalService,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISignInViewModel signInViewModel)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            CompanyNameContext = companyNameContext;
            TextTranslator = textTranslator;
            ProfileContext = profileContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SignInViewModel = signInViewModel;
        }

        public string LogoImageUrl => SiteRootContext.Item?.Site_Logo?.Src ?? string.Empty;
        public string SessionSynchronisationImageUrl => SalesforceConfigurationContext?.GetSessionSynchronisationEndPoints();

        public string LogoUrl => "/";
        public string WelcomeText
        {
            get
            {
                var user = AuthenticatedUserContext.IsAuthenticated
                         ? ProfileContext.Profile
                         : null;
                var accountName = AuthenticatedUserContext.IsAuthenticated ? user?.FirstName : !SalesforceConfigurationContext.IsNewSalesforceEnabled ? CompanyNameContext.Name : string.Empty;

                return string.IsNullOrWhiteSpace(accountName) ? string.Empty : string.Concat(TextTranslator.Translate("Header.Greeting"), accountName);
            }
        }

        public string PolicyText => TextTranslator.Translate("Global.PolicyText");
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public string MyAccountLinkText => TextTranslator.Translate("Header.MyAccount");
        public string MyAccountLink => SiteRootContext.Item?.My_Account_Page?._Url ?? "#";
        public string SignOutLinkText => TextTranslator.Translate("Header.SignOut");
        public string RegisterLinkText => TextTranslator.Translate("Header.RegisterLink");
        public string SignInText => TextTranslator.Translate("Header.SignIn");
        public string SignInLinkText => TextTranslator.Translate("Header.SignInLink");
        public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
        private IInforma_Bar InformaBar => GlobalService.GetInformaBar();
        public string LeftColumnText => InformaBar?.Left_Column_Text.Replace("#PRODUCT_NAME#", SiteRootContext.Item?.Publication_Name) ?? string.Empty;
        public string RightColumnText => InformaBar?.Right_Column_Text ?? string.Empty;
        public string Link1 => (InformaBar != null) ? BuildLink(InformaBar.Link_1) : string.Empty;
        public string Link2 => (InformaBar != null) ? BuildLink(InformaBar.Link_2) : string.Empty;
        public string Link3 => (InformaBar != null) ? BuildLink(InformaBar.Link_3) : string.Empty;
        public string Link4 => (InformaBar != null) ? BuildLink(InformaBar.Link_4) : string.Empty;

        public string PrintPageHeaderLogoSrc => SiteRootContext.Item?.Print_Logo?.Src ?? string.Empty;
        public HtmlString PrintPageHeaderMessage => new HtmlString(SiteRootContext.Item?.Print_Message);
        public HtmlString RightSubMenuContent => new HtmlString(SiteRootContext.Item?.Right_SubMenu_Content);
        public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
        public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;
        public string CorporateName => SalesforceConfigurationContext.IsNewSalesforceEnabled ? string.Empty : CompanyNameContext.Name;
        public bool UseNewSalesForce => SalesforceConfigurationContext.IsNewSalesforceEnabled;

        public string AuthorizationRequestUrl => SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), HttpContext.Current.Request.Url.ToString().Contains("?") ? HttpContext.Current.Request.Url.ToString() + "&vid=" + CurVerticalName : HttpContext.Current.Request.Url.ToString() + "?vid=" + CurVerticalName);

        public string RegistrationUrl => SalesforceConfigurationContext?.GetRegistrationEndPoints(GetCallbackUrl("/User/ProcessUserRequest/Register"), SiteRootContext?.Item?.Publication_Code);

        public string LogoutUrl => SalesforceConfigurationContext?.GetLogoutEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest/Logout") + "?vid=" + CurVerticalName);

        private string BuildLink(Link l)
        {
            if (l == null)
                return string.Empty;

            return $"<a href='{l.Url}' target='{l.Target}'>{l.Text}</a>";
        }

        public string LeaderboardSlotID
        {
            get
            {
                //ISW-338 Serving ads based on section taxonomy
                string LeaderboardSlotId = string.Empty;
                var articleModel = GlassModel as IArticle;
                if (articleModel != null)
                {
                    if (articleModel.Taxonomies != null)
                    {
                        int taxonomyItemCount = 0;
                        foreach (ITaxonomy_Item item in articleModel.Taxonomies)
                        {
                            if (item != null && taxonomyItemCount < 3)
                            {
                                LeaderboardSlotId = string.IsNullOrEmpty(LeaderboardSlotId) && !string.IsNullOrEmpty(item.Leaderboard_Slot_ID) ? item.Leaderboard_Slot_ID : LeaderboardSlotId;

                                if (!string.IsNullOrEmpty(item.Leaderboard_Slot_ID))
                                    break;
                                taxonomyItemCount++;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(LeaderboardSlotId))
                {
                    string local = string.Copy(GlassModel?.Leaderboard_Slot_ID ?? string.Empty);
                    return string.IsNullOrEmpty(local)
                        ? SiteRootContext.Item?.Global_Leaderboard_Slot_ID ?? string.Empty
                        : local;
                }
                return LeaderboardSlotId;
            }
        }

        public string LeaderboardAdZone => SiteRootContext?.Item?.Global_Leaderboard_Ad_Zone ?? string.Empty;
        public bool IsErrorMessage => !string.IsNullOrWhiteSpace(HttpContext.Current.Request.QueryString["ErrorStatus"]) && string.Equals(HttpContext.Current.Request.QueryString["ErrorStatus"].ToString(), "true", StringComparison.OrdinalIgnoreCase);
        public ISignInViewModel SignInViewModel { get; }

        private string GetCallbackUrl(string url)
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}{HttpContext.Current.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
        public string CurVerticalName => GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;

    }
}
       
              

 