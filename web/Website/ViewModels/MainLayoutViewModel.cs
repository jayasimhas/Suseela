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
using Informa.Library.SiteDebugging;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISiteDebuggingAllowedContext SiteDebuggingAllowedContext;

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
			ISiteRootContext siteRootContext,
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
            ISiteDebuggingAllowedContext siteDebuggingAllowedContext)
		{
			SiteRootContext = siteRootContext;
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

        }


        public HtmlString PrintPageHeaderMessage => new HtmlString(SiteRootContext.Item.Print_Message);
		public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
		public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;
		public string CorporateName => UserCompanyContext?.Company?.Name;
		public string Title
		{
			get
			{
				var pageTitle = GlassModel?.Meta_Title_Override.StripHtml() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(pageTitle))
					pageTitle = GlassModel?.Title?.StripHtml() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(pageTitle))
					pageTitle = GlassModel?._Name ?? string.Empty;

				var publicationName = (SiteRootContext.Item == null)
					? string.Empty
					: $" :: {SiteRootContext.Item.Publication_Name.StripHtml()}";

				return string.Concat(pageTitle, publicationName);
			}
		}
		public string BodyCssClass => string.IsNullOrEmpty(SiteRootContext.Item?.Publication_Theme)
			? string.Empty
			: $"class={SiteRootContext.Item.Publication_Theme}";
		public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();
	}
}