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
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Services.AccountManagement;
using Glass.Mapper.Sc.Fields;

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

	    public MainLayoutViewModel(IDependencies dependencies)
	    {
	        _dependencies = dependencies;
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

    }
}