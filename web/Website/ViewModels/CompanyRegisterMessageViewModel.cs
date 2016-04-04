using Informa.Library.Company;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class CompanyRegisterMessageViewModel : ICompanyRegisterMessageViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IUserCompanyContext UserCompanyContext;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

		public CompanyRegisterMessageViewModel(
			ITextTranslator textTranslator,
			ISiteRootContext siteRootContext,
			IUserCompanyContext userCompanyContext,
			IAuthenticatedUserContext authenticatedUserContext)
		{
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			UserCompanyContext = userCompanyContext;
			AuthenticatedUserContext = authenticatedUserContext;
		}

		public string CompanyName => UserCompanyContext.Company?.Name ?? string.Empty;
		public string Message => (SiteRootContext.Item?.Recognized_IP_Announcment_Text ?? string.Empty).ReplacePatternCaseInsensitive("#Company_Name#", CompanyName);
		public string DismissText => TextTranslator.Translate("Maintenance.MaintenanceDismiss");
		public bool Display => !AuthenticatedUserContext.IsAuthenticated && UserCompanyContext.Company != null;
		public string RegisterLinkText => SiteRootContext.Item?.Register_Link?.Text;
		public string RegisterLinkUrl => SiteRootContext.Item?.Register_Link?.Url;
	}
}