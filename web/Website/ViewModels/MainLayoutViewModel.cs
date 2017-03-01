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

namespace Informa.Web.ViewModels
{
    public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{

	    private readonly IDependencies _dependencies;

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
        public MainLayoutViewModel(IDependencies dependencies, ISiteRootContext siteRootContext)
	    {
	        _dependencies = dependencies;
            SiteRootContext = siteRootContext;
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
    }
}