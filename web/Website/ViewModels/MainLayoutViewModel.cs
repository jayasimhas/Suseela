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
	    protected readonly IGlobalSitecoreService GlobalService;
	    protected readonly ISiteRootContext SiteRootContext;
        
        public readonly IUserCompanyContext UserCompanyContext;
        
        public MainLayoutViewModel(
            ITextTranslator textTranslator,
            IAuthenticatedUserContext authenticatedUserContext,
            IUserCompanyContext userCompanyContext,
            ISiteDebuggingAllowedContext siteDebuggingAllowedContext,
            IGlobalSitecoreService globalService,
            ISiteRootContext siteRootContext)
		{
			TextTranslator = textTranslator;
            AuthenticatedUserContext = authenticatedUserContext;
            UserCompanyContext = userCompanyContext;
            SiteDebuggingAllowedContext = siteDebuggingAllowedContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;

		}

	    public bool IsSiteDebuggingAllowed => SiteDebuggingAllowedContext.IsAllowed;
        public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
		public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;
		public string CorporateName => UserCompanyContext?.Company?.Name;
	    public string Title => GlobalService.GetPageTitle(GlassModel);
	    public string BodyCssClass => SiteRootContext.GetBodyCssClass();
        public HtmlString PrintPageHeaderMessage => SiteRootContext.GetPrintHeaderMessage();
        public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();
	}
}