using Glass.Mapper.Sc.Fields;
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
        private string _registrationEndpoint = "{0}/registration?referralurl={1}&referralid={2}";
        private const string logoutUrl = "/secur/logout.jsp";


        public HeaderViewModel(
            IAuthenticatedUserContext authenticatedUserContext,
            IUserCompanyNameContext companyNameContext,
            ITextTranslator textTranslator,
            ISiteRootContext siteRootContext,
            IUserProfileContext profileContext,
            IGlobalSitecoreService globalService,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            CompanyNameContext = companyNameContext;
            TextTranslator = textTranslator;
            ProfileContext = profileContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public string LogoImageUrl => SiteRootContext.Item?.Site_Logo?.Src ?? string.Empty;
        public string LogoUrl => "/";
        public string WelcomeText
        {
            get
            {
                var user = AuthenticatedUserContext.IsAuthenticated
                         ? ProfileContext.Profile
                         : null;
                var accountName = AuthenticatedUserContext.IsAuthenticated ? !SalesforceConfigurationContext.IsNewSalesforceEnabled ? user?.FirstName : AuthenticatedUserContext.User?.Username : CompanyNameContext.Name;

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
        public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
        public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;
        public string CorporateName => CompanyNameContext.Name;
        public bool UseNewSalesForce => SalesforceConfigurationContext.IsNewSalesforceEnabled;

        public string AuthorizationRequestUrl => SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), HttpContext.Current.Request.Url.ToString());

        public string RegistrationUrl => string.Format(_registrationEndpoint,
            SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Service_Url?.Url,
            GetCallbackUrl("/User/ProcessUserRequest/Register"), SiteRootContext?.Item?.Publication_Code);
            

        public string LogoutUrl => string.Format("{0}{1}",
            SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Service_Url?.Url, logoutUrl);


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
                string local = string.Copy(GlassModel?.Leaderboard_Slot_ID ?? string.Empty);
                return string.IsNullOrEmpty(local)
                    ? SiteRootContext.Item?.Global_Leaderboard_Slot_ID ?? string.Empty
                    : local;
            }
        }

        public string LeaderboardAdZone => SiteRootContext?.Item?.Global_Leaderboard_Ad_Zone ?? string.Empty;

        private string GetCallbackUrl(string url)
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}{HttpContext.Current.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
    }
}
