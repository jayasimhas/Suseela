using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.User.Authentication;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Company;
using Informa.Library.Services.Global;
using Informa.Library.SiteDebugging;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.Services.AccountManagement;
using System.Collections.Generic;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using System;
using System.Configuration;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Web.ViewModels
{
    public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{

	    private readonly IDependencies _dependencies;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        //protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;

        [AutowireService(true)]
	    public interface IDependencies
	    {
            ISiteDebuggingAllowedContext SiteDebuggingAllowedContext { get; }
            ITextTranslator TextTranslator { get; }
            IAuthenticatedUserContext AuthenticatedUserContext { get; }
            IGlobalSitecoreService GlobalSitecoreService { get; }
            ISiteRootContext SiteRootContext { get; }
            IUserCompanyContext UserCompanyContext { get; }
            IHeadMetaDataGenerator HeadMetaDataGenerator { get; }
            IAccountManagementService AccountManagementService { get; }
        }

        protected readonly ISiteRootContext SiteRootContext;
        public MainLayoutViewModel(IDependencies dependencies, ISiteRootContext siteRootContext, IAuthenticatedUserContext authenticatedUserContext, ISalesforceConfigurationContext salesforceConfigurationContext, IGlobalSitecoreService globalService)
	    {
	        _dependencies = dependencies;
            SiteRootContext = siteRootContext;
            AuthenticatedUserContext = authenticatedUserContext;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            GlobalService = globalService;
            SeamlessLogin();
        }
        
	    public bool IsSiteDebuggingAllowed => _dependencies.SiteDebuggingAllowedContext.IsAllowed;
        public string PrintedByText => _dependencies.TextTranslator.Translate("Header.PrintedBy");
		public string UserName => _dependencies.AuthenticatedUserContext.User?.Name ?? string.Empty;
		public string CorporateName => _dependencies.UserCompanyContext?.Company?.Name;
	    public string Title => _dependencies.GlobalSitecoreService.GetPageTitle(GlassModel);
	    public string BodyCssClass => _dependencies.SiteRootContext.GetBodyCssClass();
        public string FavIcon => _dependencies.SiteRootContext?.Item.FavIcon?.Src;
        public HtmlString PrintPageHeaderMessage => _dependencies.SiteRootContext.GetPrintHeaderMessage();
        public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();
	    public string MetaDataHtml => _dependencies.HeadMetaDataGenerator.GetMetaHtml();
	    public bool IsRestricted => _dependencies.AccountManagementService.IsUserRestricted(GlassModel);
        public string CustomTagsHeader => _dependencies.HeadMetaDataGenerator.GetCustomTags(0);
        public string CustomTagsFooter => _dependencies.HeadMetaDataGenerator.GetCustomTags(1);
        private IVertical_Root VerticalRoot => _dependencies.GlobalSitecoreService.GetVerticalRootAncestor(GlassModel._Id);
        private Guid siteRootTemplateID => _dependencies.SiteRootContext.Item._TemplateId;
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

        public List<string> GetVerticalDomains()
        {
            List<string> verticalDomains = new List<string>();
            var rootTemplate_ID = siteRootTemplateID;
            var currentRoot_ID = _dependencies.SiteRootContext.Item._Id;
            if (VerticalRoot != null)
            {
                var siteRoots = VerticalRoot._ChildrenWithInferType;
                foreach(var root in siteRoots)
                {
                    if(root._Id != currentRoot_ID && root._TemplateId.Equals(rootTemplate_ID))
                    {
                        var siteSettings = Sitecore.Sites.SiteManager.GetSite(root._Name);
                        if (siteSettings != null)
                            verticalDomains.Add(root._Name+"|"+siteSettings.Properties["hostName"]);
                    }
                }
            }
            return verticalDomains;
        }

       public void SeamlessLogin()
            {
                // Check seamless login is enabled
                if (ConfigurationManager.AppSettings["EnableSeamlessLogin"] != "true")
                    return;
                // Check if IDE is enabled (Enabled for Agri & Maritime), if not return, else proceed
                if (!SalesforceConfigurationContext.IsNewSalesforceEnabled)
                    return;
                // Check user is authenticated, if authenticated return, else proceed
                if (AuthenticatedUserContext.IsAuthenticated)
                    return;
                //Check if the seamless login cookie exists. if not exist return, else proceed.
                if (HttpContext.Current.Request.Cookies["Seamless"] == null)
                    return;
                //Redirect user to Salesforce
                HttpContext.Current.Response.Redirect(GetAuthorizationRequestUrl(), true);
                HttpContext.Current.Response.End();
                HttpContext.Current.Response.Flush();
                return;
            }
        private string GetAuthorizationRequestUrl()
        {
            string CurVerticalName = GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;
            return SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), HttpContext.Current.Request.Url.ToString().Contains("?") ? HttpContext.Current.Request.Url.ToString() + "&vid=" + CurVerticalName : HttpContext.Current.Request.Url.ToString() + "?vid=" + CurVerticalName);
        }
        private string GetCallbackUrl(string url)
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}{HttpContext.Current.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
    }
}