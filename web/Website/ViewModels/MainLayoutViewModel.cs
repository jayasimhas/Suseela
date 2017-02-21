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
    }
}