using System;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels.SiteDebugging;
using Informa.Web.ViewModels.PopOuts;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.User.Authentication;
using Informa.Library.Utilities.References;
using System.Web;
using System.Web.Mvc;
using Informa.Library.Globalization;
using Informa.Library.Company;
using Informa.Library.Services.Global;
using Informa.Library.SiteDebugging;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{
		
		protected readonly ITextTranslator TextTranslator;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISiteDebuggingAllowedContext SiteDebuggingAllowedContext;
	    protected readonly IGlobalService GlobalService;

        public readonly IItemReferences ItemReferences;
        public readonly IUserCompanyContext UserCompanyContext;

	    public IAnalyticsViewModel AnalyticsViewModelSource
	    {
	        get
	        {
	            AnalyticsViewModel.GlassModel = GlassModel;
	            return AnalyticsViewModel;
	        }
	    }

	    private readonly IAnalyticsViewModel AnalyticsViewModel;
        public readonly IIndividualRenewalMessageViewModel IndividualRenewalMessageInfo;
        public readonly IMaintenanceViewModel MaintenanceMessage;
        public readonly ICompanyRegisterMessageViewModel CompanyRegisterMessage;
        public readonly ISignInPopOutViewModel SignInPopOutViewModel;
        public readonly IEmailArticlePopOutViewModel EmailArticlePopOutViewModel;
        public readonly IToolbarViewModel DebugToolbar;
        public readonly IRegisterPopOutViewModel RegisterPopOutViewModel;

        
        public MainLayoutViewModel(
            ITextTranslator textTranslator,
            IAuthenticatedUserContext authenticatedUserContext,
            IItemReferences itemReferences,
            IUserCompanyContext userCompanyContext,
            IAnalyticsViewModel analyticsViewModel,
            IIndividualRenewalMessageViewModel renewalInfo,
            IMaintenanceViewModel maintenanceViewModel,
			ICompanyRegisterMessageViewModel companyRegisterMessageViewModel,
			ISignInPopOutViewModel signInPopOutViewModel,
			IEmailArticlePopOutViewModel emailArticlePopOutViewModel,
            IRegisterPopOutViewModel registerPopOutViewModel,
            ISiteDebuggingAllowedContext siteDebuggingAllowedContext,
            IGlobalService globalService)
		{
			TextTranslator = textTranslator;
            AuthenticatedUserContext = authenticatedUserContext;
            ItemReferences = itemReferences;
            UserCompanyContext = userCompanyContext;
            AnalyticsViewModel = analyticsViewModel;
            IndividualRenewalMessageInfo = renewalInfo;
            MaintenanceMessage = maintenanceViewModel;
			CompanyRegisterMessage = companyRegisterMessageViewModel;
			SignInPopOutViewModel = signInPopOutViewModel;
			EmailArticlePopOutViewModel = emailArticlePopOutViewModel;
            SiteDebuggingAllowedContext = siteDebuggingAllowedContext;
            if (SiteDebuggingAllowedContext.IsAllowed)
                DebugToolbar = DependencyResolver.Current.GetService<IToolbarViewModel>();
            RegisterPopOutViewModel = registerPopOutViewModel;
            GlobalService = globalService;
		}


        public HtmlString PrintPageHeaderMessage => GlobalService.GetPrintHeaderMessage();
		public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
		public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;
		public string CorporateName => UserCompanyContext?.Company?.Name;
	    public string Title => GlobalService.GetPageTitle(GlassModel);
	    public string BodyCssClass => GlobalService.GetBodyCssClass();
		public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();
	}
}